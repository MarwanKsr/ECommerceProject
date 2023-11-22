using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApi.Models;

namespace OrderApi.DbContexts.EntitiesConfigurations
{
    public class OrderDetailsEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.HasOne(e => e.Product).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Navigation(e => e.Product).AutoInclude();

            builder.HasOne(e => e.OrderHeader).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Navigation(e => e.OrderHeader).AutoInclude();
        }
    }
}
