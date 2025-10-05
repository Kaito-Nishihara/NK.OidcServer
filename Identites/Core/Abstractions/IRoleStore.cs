using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Abstractions
{
    public interface IRoleStore<TRole, TKey> where TRole : class, IAuthRole<TKey>
    {
        Task<TRole?> FindByIdAsync(TKey id, CancellationToken ct = default);
        Task<TRole?> FindByNameAsync(string normalizedName, CancellationToken ct = default);
        Task CreateAsync(TRole role, CancellationToken ct = default);
        Task UpdateAsync(TRole role, CancellationToken ct = default);
    }
}
