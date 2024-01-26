namespace API.Configuration
{
    public static class AddRedis
    {
        public static IServiceCollection AddRedisConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =
                    configuration.GetConnectionString("ConnectionStrings:RedisConnectionString");
            });
            return services;
        }
    }
}
