using Identites.Core.Abstractions;
using Identites.Core.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Services
{
    public sealed class UserManager<TUser, TKey>
     where TUser : class, IAuthUser<TKey>
    {
        private readonly IUserStore<TUser, TKey> _users;
        private readonly ILookupNormalizer _norm;
        private readonly IPasswordHasher _hasher;

        public UserManager(IUserStore<TUser, TKey> users, ILookupNormalizer norm, IPasswordHasher hasher)
        {
            _users = users; _norm = norm; _hasher = hasher;
        }

        public async Task<Result<TUser>> CreateAsync(TUser user, string password, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(user.UserName)) return Result<TUser>.Fail(AuthErrors.UserNameRequired);
            if (string.IsNullOrWhiteSpace(password)) return Result<TUser>.Fail(AuthErrors.PasswordRequired);

            user.NormalizedUserName = _norm.Normalize(user.UserName);
            if (!string.IsNullOrEmpty(user.Email)) user.NormalizedEmail = _norm.Normalize(user.Email);

            // 重複チェック（存在すれば失敗）
            if (await _users.FindByNameAsync(user.NormalizedUserName, ct) is not null)
                return Result<TUser>.Fail(AuthErrors.DuplicateUserName);

            if (!string.IsNullOrEmpty(user.NormalizedEmail) &&
                await _users.FindByEmailAsync(user.NormalizedEmail, ct) is not null)
                return Result<TUser>.Fail(AuthErrors.DuplicateEmail);

            user.PasswordHash = _hasher.Hash(password);
            user.SecurityStamp = Guid.NewGuid().ToString("N");
            user.ConcurrencyStamp = Guid.NewGuid().ToString("N");

            await _users.CreateAsync(user, ct);
            return Result<TUser>.Success(user);
        }

        public Task<TUser?> FindByUserNameAsync(string userName, CancellationToken ct = default)
            => _users.FindByNameAsync(_norm.Normalize(userName), ct);

        public Task<TUser?> FindByEmailAsync(string email, CancellationToken ct = default)
            => _users.FindByEmailAsync(_norm.Normalize(email), ct);

        public bool CheckPassword(TUser user, string password) => _hasher.Verify(password, user.PasswordHash);

        public async Task UpdateSecurityStampAsync(TUser user, CancellationToken ct = default)
        {
            user.SecurityStamp = Guid.NewGuid().ToString("N");
            await _users.UpdateAsync(user, ct);
        }

        public async Task<Result> ChangePasswordAsync(TUser user, string currentPassword, string newPassword, CancellationToken ct = default)
        {
            if (!CheckPassword(user, currentPassword)) return Result.Fail(AuthErrors.InvalidCredentials);
            user.PasswordHash = _hasher.Hash(newPassword);
            user.SecurityStamp = Guid.NewGuid().ToString("N");
            await _users.UpdateAsync(user, ct);
            return Result.Success();
        }
    }
}
