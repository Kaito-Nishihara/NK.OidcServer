namespace OpenIddictHost.Extensions;
// Foo.Auth.OidcServer/ServiceCollectionExtensions.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddictHost.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFooOidcServer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.UseOpenIddict(); // OpenIddict エンティティ登録
        });

        services
            .AddOpenIddict()
            // Core
            .AddCore(opt =>
            {
                opt.UseEntityFrameworkCore()
                   .UseDbContext<ApplicationDbContext>();
            })
            // Server
            .AddServer(opt =>
            {
                opt
                    // エンドポイント
                    .SetAuthorizationEndpointUris("/connect/authorize")
                    .SetTokenEndpointUris("/connect/token")
                    .SetUserInfoEndpointUris("/connect/userinfo")
                    .SetIntrospectionEndpointUris("/connect/introspect");

                // 利用するフロー
                opt.AllowAuthorizationCodeFlow()
                   .RequireProofKeyForCodeExchange();   // PKCE
                opt.AllowRefreshTokenFlow();

                // 開発用証明書（本番はX509/KeyVault等）
                opt.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate();

                // scope / claim の発行設定などもここで
                //opt.RegisterScopes(Scopes.OpenId, Scopes.Email, ...);

                // ASP.NET Core 統合
                opt.UseAspNetCore()
                   .EnableAuthorizationEndpointPassthrough()
                   .EnableTokenEndpointPassthrough()
                   .EnableUserInfoEndpointPassthrough();
            });

        // 認証/認可（Cookie + OpenIddict）
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
            })
            .AddCookie("Cookies");

        services.AddAuthorization();

        return services;
    }
}
