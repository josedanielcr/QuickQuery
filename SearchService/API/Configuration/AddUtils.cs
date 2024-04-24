using QuickquerySearchAPI.Utilities;

namespace QuickquerySearchAPI.Configuration
{
    public static class AddUtils
    {
        public static IServiceCollection AddApplicationUtils(this IServiceCollection services)
        {
            services.AddScoped<CacheUtils>();
            services.AddScoped<SearchCountryCacheUtils>();
            services.AddScoped<SearchCountryHttpUtils>();
            services.AddScoped<HttpUtils>();
            services.AddScoped<CountrySearchLogUtils>();
            services.AddScoped<JwtUtils>();
            return services;
        }
    }
}
