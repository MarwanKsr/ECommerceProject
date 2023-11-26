using Microsoft.EntityFrameworkCore;
using OrderApi.DbContexts.EntitiesConfigurations;
using OrderApi.Models;

namespace OrderApi.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderHeaderOrderDetails> OrderHeaderOrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new OrderDetailsEntityTypeConfiguration());
        }
    }
}
