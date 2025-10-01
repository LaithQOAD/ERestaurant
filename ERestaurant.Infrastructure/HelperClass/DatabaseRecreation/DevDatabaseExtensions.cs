using ERestaurant.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ERestaurant.Infrastructure.HelperClass.DatabaseRecreation
{
    public static class DevDatabaseExtensions
    {
        public static async Task UseDevDatabaseRecreateAsync(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment()) return;

            var enabled = app.Configuration.GetValue<bool>("App:RecreateDatabaseOnStart");
            if (!enabled) return;

            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                                             .CreateLogger("DevDatabase");

            try
            {
                var db = scope.ServiceProvider.GetRequiredService<ERestaurantDbContext>();

                await db.Database.EnsureDeletedAsync();

                await db.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to recreate database on start.");
            }
        }
    }
}
