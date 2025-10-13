namespace Infra.Extensions;

using Identites.Core.Abstractions;
using Infra.Data.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class EntityFrameworkStoresExtensions
{ 
    public static IServiceCollection AddEntityFrameworkUserStores<TContext, TUser, TKey>(this IServiceCollection services)
        where TContext : DbContext
        where TUser : class, IAuthUser<TKey>
        where TKey : IEquatable<TKey>
    {
        services.TryAddScoped<IUserEmailStore<TUser, TKey>, UserEmailStoreEf<TUser, TKey, TContext>>();
        services.TryAddScoped<IUserPhoneStore<TUser, TKey>, UserPhoneStoreEf<TUser, TKey, TContext>>();
        services.TryAddScoped<IUserStore<TUser, TKey>, UserCoreStoreEf<TUser, TKey, TContext>>();

        return services;
    }

    public static IServiceCollection AddEntityFrameworkUserStores<TContext, TUser, TKey,
        TUserStore, TEmailStore, TPhoneStore>(
        this IServiceCollection services)
        where TContext : DbContext
        where TUser : class, IAuthUser<TKey>
        where TKey : IEquatable<TKey>
        where TUserStore : class, IUserStore<TUser, TKey>
        where TEmailStore : class, IUserEmailStore<TUser, TKey>
        where TPhoneStore : class, IUserPhoneStore<TUser, TKey>
    {
        services.AddScoped<IUserStore<TUser, TKey>, TUserStore>();
        services.AddScoped<IUserEmailStore<TUser, TKey>, TEmailStore>();
        services.AddScoped<IUserPhoneStore<TUser, TKey>, TPhoneStore>();
        return services;
    }


}
