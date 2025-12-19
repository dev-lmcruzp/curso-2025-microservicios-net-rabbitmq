using Microsoft.EntityFrameworkCore;

namespace TSquad.BookHub.Books.WebApi.Infrastructure.Persistence.Contexts;

public class ApplicationContextBookInitializer
{
    public static async Task InitialiseDataBaseAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContextBook>();

        var canConnect = await context.Database.CanConnectAsync();
        var pendingMigrations = canConnect
            ? await context.Database.GetPendingMigrationsAsync()
            : [];

        if (!pendingMigrations.Any(x => x.Contains("CreateInitial")))
            return;

        await context.Database.MigrateAsync();
    }
}