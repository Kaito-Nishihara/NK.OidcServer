using Core.Options;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOidcServerCore(
        this IServiceCollection services,
        Action<OidcServerOptions> configure)
    {
        services.AddOptions<OidcServerOptions>().Configure(configure);
        services.AddScoped<AuthorizationService>();
        services.AddScoped<TokenService>();
        services.AddScoped<DiscoveryService>();
        return services;
    }
}

