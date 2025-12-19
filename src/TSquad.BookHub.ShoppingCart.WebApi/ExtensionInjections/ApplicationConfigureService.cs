using System.Reflection;
using FluentValidation;

namespace TSquad.BookHub.ShoppingCart.WebApi.ExtensionInjections;
public static class ApplicationConfigureService
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationConfigureServices()
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
        
            return services;
        }
    }
}