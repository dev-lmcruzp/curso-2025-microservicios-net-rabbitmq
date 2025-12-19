using TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Infrastructure;
using TSquad.BookHub.ShoppingCart.WebApi.Application.Interface.Infrastructure.Services;
using TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Infrastructure.Services;

namespace TSquad.BookHub.ShoppingCart.WebApi.ExtensionInjections;

public static class InfrastructureConfigureService
{
    public static IServiceCollection AddInfrastructureConfigureService(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        return services;
    }
}