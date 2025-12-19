using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TSquad.BookHub.ShoppingCart.WebApi.Domain.Entities;
using TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Interceptors;

namespace TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Contexts;

public class ApplicationContextShoppingCart : DbContext
{
    private readonly AuditableEntitySaveChangesInterceptor? _auditableSaveChangesInterceptor;
    
    public ApplicationContextShoppingCart(DbContextOptions<ApplicationContextShoppingCart> options, 
        AuditableEntitySaveChangesInterceptor? auditableSaveChangesInterceptor) : base(options)
    {
        _auditableSaveChangesInterceptor = auditableSaveChangesInterceptor;
    }
    
    public DbSet<ShoppingCartSession>  ShoppingCartSessions { get; set; }
    public DbSet<ShoppingCartItem>  ShoppingCartItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_auditableSaveChangesInterceptor != null) optionsBuilder.AddInterceptors(_auditableSaveChangesInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}