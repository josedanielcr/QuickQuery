namespace QuickquerySearchAPI.Configuration
{
    public static class AddHttp
    {
        public static IServiceCollection AddCustomHttpClient(this IServiceCollection services)
        {
            // This is a workaround for the SSL certificate issue in the development environment.
            services.AddHttpClient("BypassSslValidation")
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            });
            return services;
        }
    }
}
