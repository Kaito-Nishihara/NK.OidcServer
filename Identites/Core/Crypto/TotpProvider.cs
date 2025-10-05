using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Crypto
{
    public sealed class TotpProvider : ITotpProvider
    {
        public string GenerateSecret()
        {
            var buf = new byte[20]; // 160-bit secret（推奨）
            RandomNumberGenerator.Fill(buf);
            return Base32.Encode(buf);
        }

        public string GenerateCode(string base32Secret, DateTimeOffset now)
        {
            var key = Base32.Decode(base32Secret);
            var steps = now.ToUnixTimeSeconds() / 30;
            var counter = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)steps));

            using var hmac = new HMACSHA1(key);
            var hash = hmac.ComputeHash(counter);

            int offset = hash[^1] & 0x0F;
            int binary = ((hash[offset] & 0x7f) << 24)
                       | (hash[offset + 1] << 16)
                       | (hash[offset + 2] << 8)
                       | (hash[offset + 3]);

            var code = (binary % 1_000_000).ToString("000000", CultureInfo.InvariantCulture);
            return code;
        }

        public bool Verify(string base32Secret, string code, DateTimeOffset now, int window = 1)
        {
            for (int w = -window; w <= window; w++)
            {
                var t = now.AddSeconds(w * 30);
                if (GenerateCode(base32Secret, t) == code) return true;
            }
            return false;
        }
    }
}
