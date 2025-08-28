using Microsoft.EntityFrameworkCore;

namespace NK.OidcServer.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
