using Microsoft.EntityFrameworkCore;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Contexts;
using TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Interceptors;
using TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Repositories;

namespace TSquad.BookHub.ShoppingCart.WebApi.ExtensionInjections;

public static class PersistenceConfigureService
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistenceConfigureServices(IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
            services.AddContext(configuration);
            return services;
        }

        private void AddContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionDataBase")!;
            services.AddDbContext<ApplicationContextShoppingCart>((p, options) =>
            {
                options.UseSqlServer(connectionString)
                    .AddInterceptors(p.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
            });
            
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        }
    }
    
    public static async Task InitialiseDataBaseAsync(this IServiceProvider services)
    {
        await ApplicationContextShoppingCartInitializer.InitialiseDataBaseAsync(services);
    }
}
