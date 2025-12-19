using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TSquad.BookHub.Books.WebApi.Domain.Entities;

namespace TSquad.BookHub.Books.WebApi.Infrastructure.Persistence.Contexts;

public class ApplicationContextBook : DbContext
{
    public DbSet<Book>  Books { get; set; }
    public ApplicationContextBook(DbContextOptions<ApplicationContextBook> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}