using CodePulseAdminPortal.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulseAdminPortal.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<BlogImage> BlogImage { get; set; }

    }
}
