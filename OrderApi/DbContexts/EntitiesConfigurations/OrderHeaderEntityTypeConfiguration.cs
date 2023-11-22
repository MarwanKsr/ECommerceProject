using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApi.Models;

namespace OrderApi.DbContexts.EntitiesConfigurations
{
    public class OrderHeaderEntityTypeConfiguration : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.Navigation(e => e.OrderDetails).AutoInclude();
        }
    }
}
