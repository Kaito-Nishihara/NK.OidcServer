using Core.Abstractions;
using Core.Extensions;
using HostApp.Components;
using Infrastructure.Extensions;
using Infrastructure.Security;
using Infrastructure.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.Cookie.Name = ".oidc.sid";
    o.IdleTimeout = TimeSpan.FromHours(8);
    o.Cookie.HttpOnly = true;
    o.Cookie.SecurePolicy = CookieSecurePolicy.Always; // dev‚È‚ç None ‚Å‚àOK
});
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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
