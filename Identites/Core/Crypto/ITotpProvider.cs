using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Crypto
{
    public interface ITotpProvider
    {
        string GenerateSecret(); // Base32
        string GenerateCode(string base32Secret, DateTimeOffset now);
        bool Verify(string base32Secret, string code, DateTimeOffset now, int window = 1);
    }
}
