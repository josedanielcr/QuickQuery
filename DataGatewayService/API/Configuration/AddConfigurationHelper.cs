using QuickqueryDataGatewayAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace QuickqueryDataGatewayAPI.Configuration
{
    public static class AddConfigurationHelper
    {
        public static IConfiguration config;
        public static void Initialize(IConfiguration Configuration)
        {
            config = Configuration;
        }
    }
}
