using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Abstractions
{
    public interface IUserEmailStore<TUser, TKey> where TUser : class, IAuthUser<TKey>
    {
        Task<TUser?> FindByEmailAsync(string normalizedEmail, CancellationToken ct = default);

        Task SetEmailAsync(TUser user, string? email, CancellationToken ct = default);
        Task<string?> GetEmailAsync(TUser user, CancellationToken ct = default);

        Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken ct = default);
        Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken ct = default);
    }

}
