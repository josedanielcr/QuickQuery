namespace QuickquerySearchAPI.Configuration
{
    public static class AddCORS
    {
        public static IServiceCollection AddApplicationCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Policy", builder =>
                {
                    builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
                });
            });
            return services;
        }
    }
}
