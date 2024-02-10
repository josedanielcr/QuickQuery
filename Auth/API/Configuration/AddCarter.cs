using Carter;

namespace QuickqueryAuthenticationAPI.Configuration
{
    public static class AddCarter
    {
        public static IServiceCollection AddApplicationCarter(this IServiceCollection services)
        {
            services.AddCarter();
            return services;
        }
    }
}
