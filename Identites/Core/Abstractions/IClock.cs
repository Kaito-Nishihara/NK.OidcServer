using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Abstractions
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }

    public sealed class SystemClock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
