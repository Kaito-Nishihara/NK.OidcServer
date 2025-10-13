using Core.Abstractions;
using Core.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Security;
using Infrastructure.Stores;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOidcServerCore(opt =>
{
    opt.Issuer = "https://issuer.example.com";
    opt.DefaultAudience = "your-api";
});

builder.Services
    .AddInMemoryClients(new OidcClient
    {
        ClientId = "spa",
        RedirectUris = new(StringComparer.Ordinal) { "https://app.example.com/callback" },
        AllowedScopes = new(StringComparer.Ordinal) { "openid", "profile", "email", "offline_access" },
        RequirePkce = true,
        AllowOfflineAccess = true
    })
    .AddAuthorizationCodeStore<InMemoryAuthorizationCodeStore>();
builder.Services.AddAuthentication("Cookies").AddCookie("Cookies");
builder.Services.AddAuthorization();
builder.Services.AddScoped<ISubjectResolver, ClaimsPrincipalSubjectResolver>();
builder.Services.AddSingleton<IJwtIssuerBridge, RsaJwtIssuerBridge>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
