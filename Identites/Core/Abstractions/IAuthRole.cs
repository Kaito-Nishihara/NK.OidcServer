using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Abstractions
{
    public interface IAuthRole<TKey>
    {
        TKey Id { get; }
        string Name { get; set; }
        string NormalizedName { get; set; }
    }
}
