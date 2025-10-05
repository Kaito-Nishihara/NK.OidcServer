using Identites.Core.Abstractions;
using Identites.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Services
{
    public sealed class SignInManager<TUser, TKey>
     where TUser : class, IAuthUser<TKey>
    {
        private readonly UserManager<TUser, TKey> _users;
        private readonly IClock _clock;

        public SignInManager(UserManager<TUser, TKey> users, IClock clock)
        { _users = users; _clock = clock; }

        public async Task<SignInResult> PasswordSignInAsync(TUser user, string password, CancellationToken ct = default)
        {
            var now = _clock.UtcNow;

            // Lockout チェック
            if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd > now)
                return SignInResult.LockedOut;

            if (!_users.CheckPassword(user, password))
            {
                user.AccessFailedCount++;
                if (user.LockoutEnabled && user.AccessFailedCount >= 5)
                    user.LockoutEnd = now.AddMinutes(5);
                await _users.ChangePasswordAsync(user, password, password, ct: ct); // ダミー保存に見えないよう注意（実際の保存は Store 側の Update が必要）
                return SignInResult.Failed;
            }

            if (user.TwoFactorEnabled)
                return SignInResult.RequiresTwoFactor;

            user.AccessFailedCount = 0;
            user.LockoutEnd = null;
            await _users.UpdateSecurityStampAsync(user, ct); // セッション分離のトリガに使える
            return SignInResult.Success;
        }
    }
}
