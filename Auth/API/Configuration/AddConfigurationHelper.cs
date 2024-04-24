using QuickqueryAuthenticationAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace QuickqueryAuthenticationAPI.Configuration
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
