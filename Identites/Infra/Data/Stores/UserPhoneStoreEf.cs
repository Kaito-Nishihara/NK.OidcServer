// Infra/Data/Stores/UserPhoneStoreEf.cs
namespace Infra.Data.Stores;

using Identites.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

public class UserPhoneStoreEf<TUser, TKey, TContext> : IUserPhoneStore<TUser, TKey>
    where TUser : class, IAuthUser<TKey>
    where TKey : IEquatable<TKey>
    where TContext : DbContext
{
    private readonly TContext _db;
    private readonly DbSet<TUser> _users;

    public UserPhoneStoreEf(TContext db)
    {
        _db = db;
        _users = _db.Set<TUser>();
    }

    public Task<TUser?> FindByPhoneAsync(string phoneNumber, CancellationToken ct = default)
        => _users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, ct);

    public Task SetPhoneNumberAsync(TUser user, string? number, CancellationToken ct = default)
    {
        user.PhoneNumber = number;
        return Task.CompletedTask;
    }

    public Task<string?> GetPhoneNumberAsync(TUser user, CancellationToken ct = default)
        => Task.FromResult(user.PhoneNumber);

    public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken ct = default)
        => Task.FromResult(user.PhoneNumberConfirmed);

    public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken ct = default)
    {
        user.PhoneNumberConfirmed = confirmed;
        return Task.CompletedTask;
    }
}
