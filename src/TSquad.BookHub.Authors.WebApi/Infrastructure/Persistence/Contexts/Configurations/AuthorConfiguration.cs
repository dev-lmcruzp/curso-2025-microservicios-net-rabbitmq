using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSquad.BookHub.Authors.WebApi.Domain.Entities;

namespace TSquad.BookHub.Authors.WebApi.Infrastructure.Persistence.Contexts.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors")
            .HasKey(c => c.Id)
            .HasName("PK_Authors");

        builder.Property(x => x.Id)
            .HasColumnName("AuthorId")
            .IsRequired();
        
        builder.Property(c => c.ExternalAuthorId)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(c => c.Surnames)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(p => p.DateOfBirth)
            .HasColumnType("date");
    }
}