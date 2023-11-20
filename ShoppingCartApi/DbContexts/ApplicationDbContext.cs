using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.DbContexts.EntitiesConfigurations;
using ShoppingCartApi.Models;

namespace ShoppingCartApi.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CardHeader> CartHeaders { get; set; }
        public DbSet<CardDetails> CartDetails { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            builder.ApplyConfiguration(new CardDetailsEntityTypeConfiguration());
        }
    }
}
