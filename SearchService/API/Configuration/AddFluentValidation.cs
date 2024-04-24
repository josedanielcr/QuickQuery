using FluentValidation;

namespace QuickquerySearchAPI.Configuration
{
    public static class AddFluentValidation
    {
        public static IServiceCollection AddApplicationFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            return services;
        }
    }
}
