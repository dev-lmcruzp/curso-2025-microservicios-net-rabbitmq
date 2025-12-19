using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSquad.BookHub.Books.WebApi.Domain.Entities;

namespace TSquad.BookHub.Books.WebApi.Infrastructure.Persistence.Contexts.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books")
            .HasKey(x => x.Id)
            .HasName("PK_Books");

        builder.Property(x => x.Id)
            .HasColumnName("BookId")
            .IsRequired();
        
        builder.Property(x => x.ExternalBookId)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.DatePublic)
            .HasColumnType("Date");

        builder.Property(x => x.AuthorId)
            .HasMaxLength(50)
            .IsRequired();
    }
}