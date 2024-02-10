namespace QuickqueryDataGatewayAPI.Configuration
{
    public static class AddMediatR
    {
        public static IServiceCollection AddApplicationMediatR(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
            }); 

            return services;
        }
    }
}
