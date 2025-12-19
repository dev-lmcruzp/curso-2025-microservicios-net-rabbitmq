using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TSquad.BookHub.Authors.WebApi.Domain.Entities;

namespace TSquad.BookHub.Authors.WebApi.Infrastructure.Persistence.Contexts;

public class ApplicationContextAuthor : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<AcademicDegree> AcademicDegrees { get; set; }

    public ApplicationContextAuthor(DbContextOptions<ApplicationContextAuthor> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}