using TSquad.BookHub.Api.Gateway.Application.Interface;
using TSquad.BookHub.Api.Gateway.Infrastructure.Services;

namespace TSquad.BookHub.Api.Gateway.ExtensionInjections;

public static class InfrastructureConfigureService
{
    public static IServiceCollection AddInfrastructureConfigureService(this IServiceCollection services)
    {
        // Se agrega de essta forma por que se usa en un middleware
        services.AddSingleton<IAuthorService, AuthorService>();
        return services;
    }
}