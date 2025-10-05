// Infra/Data/Stores/UserStoreEf.cs （抜粋）
using Identites.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Stores;

public sealed class UserStoreEf<TUser, TKey, TContext> :
    IUserStore<TUser, TKey>,
    IUserEmailStore<TUser, TKey>,
    IUserPhoneStore<TUser, TKey>
    where TUser : class, IAuthUser<TKey>
    where TContext : DbContext
{
    private readonly TContext _db;
    private readonly DbSet<TUser> _users;
    private readonly ILookupNormalizer _norm;

    public UserStoreEf(TContext db, ILookupNormalizer norm)
    {
        _db = db;
        _users = _db.Set<TUser>();
        _norm = norm;
    }

    // --- 検索 ---
    public Task<TUser?> FindByIdAsync(TKey id, CancellationToken ct = default)
        => _users.FirstOrDefaultAsync(u => u.Id!.Equals(id), ct);

    public Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken ct = default)
        => _users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, ct);

    public Task<TUser?> FindByEmailAsync(string normalizedEmail, CancellationToken ct = default)
        => _users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, ct);

    public Task<TUser?> FindByPhoneAsync(string phoneNumber, CancellationToken ct = default)
        => _users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, ct);

    // --- 変更 & 保存 ---
    public async Task CreateAsync(TUser user, CancellationToken ct = default)
    { _users.Add(user); await _db.SaveChangesAsync(ct); }

    public async Task UpdateAsync(TUser user, CancellationToken ct = default)
    { _users.Update(user); await _db.SaveChangesAsync(ct); }

    // --- Email ---
    public Task SetEmailAsync(TUser user, string? email, CancellationToken ct = default)
    {
        user.Email = email;
        user.NormalizedEmail = _norm.Normalize(email);
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(TUser user, CancellationToken ct = default)
        => Task.FromResult(user.Email);

    public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken ct = default)
        => Task.FromResult(user.EmailConfirmed);

    public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken ct = default)
    { user.EmailConfirmed = confirmed; return Task.CompletedTask; }

    // --- Phone ---
    public Task SetPhoneNumberAsync(TUser user, string? number, CancellationToken ct = default)
    { user.PhoneNumber = number; return Task.CompletedTask; }

    public Task<string?> GetPhoneNumberAsync(TUser user, CancellationToken ct = default)
        => Task.FromResult(user.PhoneNumber);

    public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken ct = default)
        => Task.FromResult(user.PhoneNumberConfirmed);

    public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken ct = default)
    { user.PhoneNumberConfirmed = confirmed; return Task.CompletedTask; }
}
