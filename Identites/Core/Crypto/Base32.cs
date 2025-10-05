using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Crypto
{
    internal static class Base32
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static string Encode(ReadOnlySpan<byte> data)
        {
            if (data.Length == 0) return string.Empty;
            var output = new StringBuilder((data.Length + 4) / 5 * 8);

            int buffer = 0, bitsLeft = 0;
            foreach (var b in data)
            {
                buffer = buffer << 8 | b;
                bitsLeft += 8;
                while (bitsLeft >= 5)
                {
                    output.Append(Alphabet[buffer >> bitsLeft - 5 & 31]);
                    bitsLeft -= 5;
                }
            }
            if (bitsLeft > 0)
            {
                output.Append(Alphabet[buffer << 5 - bitsLeft & 31]);
            }
            return output.ToString();
        }

        public static byte[] Decode(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return Array.Empty<byte>();
            s = s.Trim().Replace("=", string.Empty).ToUpperInvariant();

            int buffer = 0, bitsLeft = 0;
            var bytes = new List<byte>(s.Length * 5 / 8);

            foreach (var c in s)
            {
                int val = Alphabet.IndexOf(c);
                if (val < 0) throw new FormatException($"Invalid Base32 char: {c}");

                buffer = buffer << 5 | val;
                bitsLeft += 5;
                if (bitsLeft >= 8)
                {
                    bytes.Add((byte)(buffer >> bitsLeft - 8 & 0xFF));
                    bitsLeft -= 8;
                }
            }
            return bytes.ToArray();
        }
    }
}
