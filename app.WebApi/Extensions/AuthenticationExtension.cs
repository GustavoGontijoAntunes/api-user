using app.Domain.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace app.WebApi.Extension
{
    internal static class AuthenticationExtension
    {
        private static readonly string JwtSection = "jwt";

        public static void AddJwt(this IServiceCollection services)
        {
            IConfiguration configuration;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var jwtOptions = new JwtOptions();
            configuration.GetSection(JwtSection).Bind(jwtOptions);

            // Inject JWT Option
            services.Configure<JwtOptions>(configuration.GetSection(JwtSection));
            services.AddSingleton<JwtOptions>();

            services.AddAuthentication
                (x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.ValidAudience,
                        ValidateAudience = jwtOptions.ValidateAudience,
                        ValidateLifetime = jwtOptions.ValidateLifetime,
                    };
                });
        }
    }
}