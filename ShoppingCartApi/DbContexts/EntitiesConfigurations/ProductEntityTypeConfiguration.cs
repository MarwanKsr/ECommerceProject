using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Models;

namespace ShoppingCartApi.DbContexts.EntitiesConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(e => e.Image).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Navigation(e => e.Image).AutoInclude();
        }
    }
}
