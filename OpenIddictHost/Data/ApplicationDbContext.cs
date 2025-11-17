
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;
namespace OpenIddictHost.Data;
public class ApplicationDbContext : DbContext // or IdentityDbContext<...>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<OpenIddictEntityFrameworkCoreApplication> Applications { get; set; } = default!;
    public DbSet<OpenIddictEntityFrameworkCoreAuthorization> Authorizations { get; set; } = default!;
    public DbSet<OpenIddictEntityFrameworkCoreScope> Scopes { get; set; } = default!;
    public DbSet<OpenIddictEntityFrameworkCoreToken> Tokens { get; set; } = default!;
}
