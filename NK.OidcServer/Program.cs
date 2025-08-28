using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NK.OidcServer.Models;
using OpenIddict.Abstractions;
using Quartz;
using System.Globalization;
using System.Text.Json.Nodes;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Configure the context to use an in-memory store.
    options.UseSqlite($"Filename={Path.Combine(Path.GetTempPath(), "openiddict-contruum-server.sqlite3")}");

    // Register the entity sets needed by OpenIddict.
    options.UseOpenIddict();
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(options =>
           {
               options.AccessDeniedPath = "/connect/signin";
               options.LoginPath = "/connect/signin";
               options.LogoutPath = "/connect/signout";
           });
builder.Services.AddQuartz(options =>
{
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenIddict()
            .AddCore(options =>
            {
                // Register the Entity Framework Core models/stores.
                options.UseEntityFrameworkCore()
                       .UseDbContext<ApplicationDbContext>();

                // Enable Quartz.NET integration.
                options.UseQuartz();
            })

            .AddServer(options =>
            {
                // Enable the authorization, token, introspection and userinfo endpoints.
                options.SetAuthorizationEndpointUris(builder.Configuration["OpenIddict:Endpoints:Authorization"]!)
                       .SetTokenEndpointUris(builder.Configuration["OpenIddict:Endpoints:Token"]!)
                       .SetIntrospectionEndpointUris(builder.Configuration["OpenIddict:Endpoints:Introspection"]!)
                       .SetUserInfoEndpointUris(builder.Configuration["OpenIddict:Endpoints:Userinfo"]!)
                       .SetEndSessionEndpointUris(builder.Configuration["OpenIddict:Endpoints:Logout"]!);

                // Enable the authorization code, implicit, hybrid and the refresh token flows.
                options.AllowAuthorizationCodeFlow()
                       .AllowImplicitFlow()
                       .AllowHybridFlow()
                       .AllowRefreshTokenFlow();

                // Expose all the supported claims in the discovery document.
                options.RegisterClaims(builder.Configuration.GetSection("OpenIddict:Claims").Get<string[]>()!);

                // Expose all the supported scopes in the discovery document.
                options.RegisterScopes(builder.Configuration.GetSection("OpenIddict:Scopes").Get<string[]>()!);

                // Note: an ephemeral signing key is deliberately used to make the "OP-Rotation-OP-Sig"
                // test easier to run as restarting the application is enough to rotate the keys.
                options.AddEphemeralEncryptionKey()
                       .AddEphemeralSigningKey();

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                //
                // Note: the pass-through mode is not enabled for the token endpoint
                // so that token requests are automatically handled by OpenIddict.
                options.UseAspNetCore()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableEndSessionEndpointPassthrough();

                // Register the custom event handler responsible for populating userinfo responses.
                options.AddEventHandler<HandleUserInfoRequestContext>(options => options.UseInlineHandler(static context =>
                {
                    if (context.AccessTokenPrincipal.HasScope(Scopes.Profile))
                    {
                        context.GivenName = context.AccessTokenPrincipal.GetClaim(Claims.GivenName);
                        context.FamilyName = context.AccessTokenPrincipal.GetClaim(Claims.FamilyName);
                        context.BirthDate = context.AccessTokenPrincipal.GetClaim(Claims.Birthdate);
                        context.Claims[Claims.Locale] = context.AccessTokenPrincipal.GetClaim(Claims.Locale);
                        context.Claims[Claims.Zoneinfo] = context.AccessTokenPrincipal.GetClaim(Claims.Zoneinfo);
                        context.Claims[Claims.UpdatedAt] = long.Parse(
                            context.AccessTokenPrincipal.GetClaim(Claims.UpdatedAt)!,
                            NumberStyles.Number, CultureInfo.InvariantCulture);
                    }

                    if (context.AccessTokenPrincipal.HasScope(Scopes.Email))
                    {
                        context.Email = context.AccessTokenPrincipal.GetClaim(Claims.Email);
                        context.EmailVerified = false;
                    }
                    if (context.AccessTokenPrincipal.HasScope(Scopes.Phone))
                    {
                        context.PhoneNumber = context.AccessTokenPrincipal.GetClaim(Claims.PhoneNumber);
                        context.PhoneNumberVerified = false;
                    }
                    if (context.AccessTokenPrincipal.HasScope(Scopes.Address))
                    {
                        context.Address = JsonNode.Parse(context.AccessTokenPrincipal.GetClaim(Claims.Address)!)!.AsObject();
                    }

                    return default;
                }));

            })
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();
            
                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            
                // Enable authorization entry validation, which is required to be able
                // to reject access tokens retrieved from a revoked authorization code.
                options.EnableAuthorizationEntryValidation();
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
