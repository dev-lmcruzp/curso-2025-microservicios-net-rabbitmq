using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities;

namespace TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Contexts.Configurations;

public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder.ToTable("ShoppingCartItems")
            .HasKey(x => x.Id)
            .HasName("PK_ShoppingCartItems");

        builder.Property(x => x.Id)
            .HasColumnName("ShoppingCartItemId")
            .IsRequired();
        
        builder.Property(x => x.ShoppingCartSessionId)
            .IsRequired();
        
        builder
            .HasOne(x => x.ShoppingCartSession)
            .WithMany(w => w.ShoppingCartItems)
            .HasForeignKey(a => a.ShoppingCartSessionId)
            .HasConstraintName("FK_ShoppingCartItems_ShoppingCartSessions")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.Property(x => x.ProductId)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}