using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Abstractions
{
    public interface IUserPhoneStore<TUser, TKey> where TUser : class, IAuthUser<TKey>
    {
        Task<TUser?> FindByPhoneAsync(string phoneNumber, CancellationToken ct = default);
        Task SetPhoneNumberAsync(TUser user, string? number, CancellationToken ct = default);
        Task<string?> GetPhoneNumberAsync(TUser user, CancellationToken ct = default);
        Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken ct = default);
        Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken ct = default);
    }
}
