using Microsoft.EntityFrameworkCore;
using ProductApi.DbContexts.EntitiesConfigurations;
using ProductApi.Models;

namespace ProductApi.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _context;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        }
    }
}
