using Identites.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Stores;

public class UserCoreStoreEf<TUser, TKey, TContext>
    : UserCoreStoreBase<TUser, TKey, TContext>
    where TUser : class, IAuthUser<TKey>
    where TContext : DbContext
{
    public UserCoreStoreEf(TContext db) : base(db) { }
}
