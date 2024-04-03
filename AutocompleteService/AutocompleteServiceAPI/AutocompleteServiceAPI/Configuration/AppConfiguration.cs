using AutocompleteServiceAPI.Features;

namespace AutocompleteServiceAPI.Configuration
{
    public static class AppConfiguration
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services)
        {
            services.AddScoped<HttpUtils>();
            services.AddScoped<TrieInitialization>();
            return services;
        }
    }
}
