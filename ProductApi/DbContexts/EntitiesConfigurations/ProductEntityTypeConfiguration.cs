using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.DbContexts.EntitiesConfigurations
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
