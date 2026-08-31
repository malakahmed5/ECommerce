using ECommerce.Domain.Contracts;
using ECommerce.Persistence.Data.Contetxts;
using ECommerce.Persistence.IdentityData.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Extentions
{
    public static class WebApplicationRegister
    {
        public async static Task<WebApplication> MigrateDataBaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations?.Any() ?? false)
                dbContext.Database.Migrate();
            return app;
        }
        public async static Task<WebApplication> MigrateIdentityDataBaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations?.Any() ?? false)
                dbContext.Database.Migrate();
            return app;
        }


        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dataInitializer = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("default");
            await dataInitializer.InitializeAsync();
            return app;
        }
        public static async Task<WebApplication> SeedIDentityDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dataInitializer = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("identity");
            await dataInitializer.InitializeAsync();
            return app;
        }
    }
}
