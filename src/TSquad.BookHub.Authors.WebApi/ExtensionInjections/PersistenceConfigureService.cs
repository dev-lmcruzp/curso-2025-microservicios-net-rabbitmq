using Microsoft.EntityFrameworkCore;
using TSquad.BookHub.Authors.WebApi.Application.Interface.Persistence;
using TSquad.BookHub.Authors.WebApi.Infrastructure.Persistence.Contexts;
using TSquad.BookHub.Authors.WebApi.Infrastructure.Persistence.Repositories;

namespace TSquad.BookHub.Authors.WebApi.ExtensionInjections;

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
        
        // Infrastructure/Persistence

        private void AddContext(IConfiguration configuration)
        {
            Console.WriteLine(configuration.GetConnectionString("ConnectionDataBase"));
            services.AddDbContext<ApplicationContextAuthor>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ConnectionDataBase"));
            });
        }
    }
    
    public static async Task InitialiseDataBaseAsync(this IServiceProvider services)
    {
        await ApplicationContextAuthorInitializer.InitialiseDataBaseAsync(services);
    }
}
