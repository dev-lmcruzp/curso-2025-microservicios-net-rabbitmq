using Microsoft.EntityFrameworkCore;

namespace TSquad.BookHub.ShoppingCart.WebApi.Infrastructure.Persistence.Contexts;

public class ApplicationContextShoppingCartInitializer
{
    public static async Task InitialiseDataBaseAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContextShoppingCart>();

        var canConnect = await context.Database.CanConnectAsync();
        var pendingMigrations = canConnect
            ? await context.Database.GetPendingMigrationsAsync()
            : [];

        if (!pendingMigrations.Any(x => x.Contains("CreateInitial")))
            return;

        await context.Database.MigrateAsync();
    }
}