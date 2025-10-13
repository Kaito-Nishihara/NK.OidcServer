using Core.Abstractions;
using Infrastructure.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;
public static class OidcInfraExtensions
{
    public static IServiceCollection AddInMemoryClients(this IServiceCollection s, params OidcClient[] clients)
        => s.AddSingleton<IClientStore>(new InMemoryClientStore(clients));

    public static IServiceCollection AddAuthorizationCodeStore<TStore>(this IServiceCollection s)
        where TStore : class, IAuthorizationCodeStore
        => s.AddScoped<IAuthorizationCodeStore, TStore>();
}