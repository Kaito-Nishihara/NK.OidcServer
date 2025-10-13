// Infra/Data/Stores/UserEmailStoreEf.cs
namespace Infra.Data.Stores;

using Identites.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

public class UserEmailStoreEf<TUser, TKey, TContext> : IUserEmailStore<TUser, TKey>
    where TUser : class, IAuthUser<TKey>
    where TKey : IEquatable<TKey>
    where TContext : DbContext
{
    private readonly TContext _db;
    private readonly DbSet<TUser> _users;
    private readonly ILookupNormalizer _norm;

    public UserEmailStoreEf(TContext db, ILookupNormalizer norm)
    {
        _db = db;
        _users = _db.Set<TUser>();
        _norm = norm;
    }

    public Task<TUser?> FindByEmailAsync(string normalizedEmail, CancellationToken ct = default)
        => _users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, ct);

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
    {
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

}
