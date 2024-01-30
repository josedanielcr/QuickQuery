namespace API.Configuration
{
    public static class AddRedis
    {
        public static IServiceCollection AddRedisConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            string connectionString 
                = configuration.GetSection("ConnectionStrings:RedisConnectionString").Value!;

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
            });
            return services;
        }
    }
}
