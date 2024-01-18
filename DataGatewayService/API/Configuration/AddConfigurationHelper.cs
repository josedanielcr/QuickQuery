using API.Database;
using Microsoft.EntityFrameworkCore;

namespace API.Configuration
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
