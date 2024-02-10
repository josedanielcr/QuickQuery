using QuickqueryDataGatewayAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace QuickqueryDataGatewayAPI.Configuration
{
    public static class ApplicationDb
    {
        public static IServiceCollection AddApplicationDb(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                           options.UseNpgsql(connectionString));

            return services;
        }
    }
}