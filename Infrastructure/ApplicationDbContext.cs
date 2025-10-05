using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }

        // OpenIddict 用のエンティティを登録
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppRoleConfiguration).Assembly);
            // OpenIddict のエンティティを登録
            modelBuilder.UseOpenIddict();
        }
    }
}
