using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace QuickqueryDataGatewayAPI.Configuration
{
    public static class AddJwtValidation
    {
        public static IServiceCollection AddApplicationJwtValidation(this IServiceCollection services,
            IConfiguration Configuration)
        {
            string key = GetJwtKey(Configuration);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(key)
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

            return services;
        }

        private static string GetJwtKey(IConfiguration Configuration)
        {
            string key = Configuration["Jwt:Secret"]!;

            if (string.IsNullOrEmpty(key))
                throw new Exception("Jwt:Secret is not set in appsettings.json");
            return key;
        }
    }
}
