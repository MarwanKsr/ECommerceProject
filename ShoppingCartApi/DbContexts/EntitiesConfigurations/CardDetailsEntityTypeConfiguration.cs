using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Models;

namespace ShoppingCartApi.DbContexts.EntitiesConfigurations
{
    public class CardDetailsEntityTypeConfiguration : IEntityTypeConfiguration<CardDetails>
    {
        public void Configure(EntityTypeBuilder<CardDetails> builder)
        {
            builder.HasOne(e => e.Product).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Navigation(e => e.Product).AutoInclude();
            
            builder.HasOne(e => e.CardHeader).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Navigation(e => e.CardHeader).AutoInclude();
        }
    }
}
