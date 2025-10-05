using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Crypto
{
    public sealed class Pbkdf2PasswordHasher : IPasswordHasher
    {
        public sealed record Options(int Iterations = 100_000, int SaltSize = 16, int KeySize = 32);

        private readonly Options _opt;
        public Pbkdf2PasswordHasher(Options? opt = null) => _opt = opt ?? new();

        public string Hash(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Password is empty.", nameof(password));

            Span<byte> salt = stackalloc byte[_opt.SaltSize];
            RandomNumberGenerator.Fill(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt.ToArray(), _opt.Iterations, HashAlgorithmName.SHA256);
            var key = pbkdf2.GetBytes(_opt.KeySize);

            return $"pbkdf2|sha256|{_opt.Iterations}|{Convert.ToBase64String(salt)}|{Convert.ToBase64String(key)}";
        }

        public bool Verify(string password, string hash)
        {
            var parts = hash.Split('|');
            if (parts.Length != 5 || parts[0] != "pbkdf2") return false;

            var iterations = int.Parse(parts[2], CultureInfo.InvariantCulture);
            var salt = Convert.FromBase64String(parts[3]);
            var expected = Convert.FromBase64String(parts[4]);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var actual = pbkdf2.GetBytes(expected.Length);

            return CryptographicOperations.FixedTimeEquals(expected, actual);
        }
    }
}
