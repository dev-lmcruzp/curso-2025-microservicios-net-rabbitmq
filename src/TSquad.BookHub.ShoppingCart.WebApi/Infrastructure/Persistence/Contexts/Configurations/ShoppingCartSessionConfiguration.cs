using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities;

namespace TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Contexts.Configurations;

public class ShoppingCartSessionConfiguration : IEntityTypeConfiguration<ShoppingCartSession>
{
    public void Configure(EntityTypeBuilder<ShoppingCartSession> builder)
    {
        builder.ToTable("ShoppingCartSessions")
            .HasKey(x => x.Id)
            .HasName("PK_ShoppingCartSessions");

        builder.Property(x => x.Id)
            .HasColumnName("ShoppingCartSessionId")
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}