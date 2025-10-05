using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Abstractions
{
    public interface ILookupNormalizer
    {
        string Normalize(string? value);
    }

    public sealed class UpperInvariantNormalizer : ILookupNormalizer
    {
        public string Normalize(string? value) => (value ?? string.Empty).ToUpperInvariant();
    }
}
