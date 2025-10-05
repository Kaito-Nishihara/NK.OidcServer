// Infra/Data/Stores/Base/UserCoreStoreBase.cs
using Identites.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Stores;

public abstract class UserCoreStoreBase<TUser, TKey, TContext> : IUserStore<TUser, TKey>
    where TUser : class, IAuthUser<TKey>
    where TContext : DbContext
{
    protected readonly TContext Db;
    protected readonly DbSet<TUser> Users;

    protected UserCoreStoreBase(TContext db)
    { Db = db; Users = Db.Set<TUser>(); }

    public virtual Task<TUser?> FindByIdAsync(TKey id, CancellationToken ct = default)
        => Users.FirstOrDefaultAsync(u => u.Id!.Equals(id), ct);

    public virtual Task<TUser?> FindByNameAsync(string normalizedUserName, CancellationToken ct = default)
        => Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, ct);

    public virtual async Task CreateAsync(TUser user, CancellationToken ct = default)
    { Users.Add(user); await Db.SaveChangesAsync(ct); }

    public virtual async Task UpdateAsync(TUser user, CancellationToken ct = default)
    { Users.Update(user); await Db.SaveChangesAsync(ct); }

    // Hook: 追加の拡張点を用意しておくと継承が楽
    protected virtual Task OnBeforeCreateAsync(TUser user, CancellationToken ct) => Task.CompletedTask;
    protected virtual Task OnAfterCreateAsync(TUser user, CancellationToken ct) => Task.CompletedTask;
}
