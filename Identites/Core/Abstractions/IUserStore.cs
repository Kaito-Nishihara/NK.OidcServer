using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Abstractions
{
    public interface IUserStore<TUser, TKey> where TUser : class, IAuthUser<TKey>
    {
        Task<TUser?> FindByIdAsync(TKey id, CancellationToken ct = default);
        Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken ct = default);
        Task CreateAsync(TUser user, CancellationToken ct = default);
        Task UpdateAsync(TUser user, CancellationToken ct = default);
    }
}
