using WebApi.Infrastructure.Context;

namespace WebApi.Infrastructure.Extensions;

public static class DatabaseMigrationExtensions
{
    public static async Task<IApplicationBuilder> MigrateDatabaseAsync(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<TechChallenge1DbContext>();
            await dbContext.MigrateAsync();
        }

        return app;
    }
}