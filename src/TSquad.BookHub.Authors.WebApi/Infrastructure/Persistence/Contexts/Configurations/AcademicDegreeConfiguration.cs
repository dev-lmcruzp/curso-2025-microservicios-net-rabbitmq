using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSquad.BookHub.Authors.WebApi.Domain.Entities;

namespace TSquad.BookHub.Authors.WebApi.Infrastructure.Persistence.Contexts.Configurations;

public class AcademicDegreeConfiguration : IEntityTypeConfiguration<AcademicDegree>
{
    public void Configure(EntityTypeBuilder<AcademicDegree> builder)
    {
        builder.ToTable("AcademicDegrees")
            .HasKey(a => a.Id)
            .HasName("PK_AcademicDegrees");

        builder.Property(a => a.Id)
            .HasColumnName("AcademicDegreeId")
            .IsRequired();

        builder
            .HasOne(x => x.Author)
            .WithMany(w => w.AcademicDegrees)
            .HasForeignKey(a => a.AuthorId)
            .HasConstraintName("FK_AcademicDegrees_Authors")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.Property(a => a.Name)
            .HasMaxLength(150)
            .IsRequired();
        
        builder.Property(a => a.ExternalAcademicDegreeId)
            .HasMaxLength(250)
            .IsRequired();
        
        builder.Property(a => a.AcademicInstitution)
            .HasMaxLength(250)
            .IsRequired();
    }
}