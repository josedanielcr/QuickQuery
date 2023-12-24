using API.Database;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplyMigrations
    {
        public static void ApplyDatabaseMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }
    }
}