using app.Domain.Resources;
using app.WebApi.Middlewares;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System.Globalization;

namespace app.WebApi.Configuration
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddLocalization(options => options.ResourcesPath = "");
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo(CultureName.PT_BR),
                        new CultureInfo(CultureName.ES_AR),
                        new CultureInfo(CultureName.EN_US)
                    };

                    options.DefaultRequestCulture = new RequestCulture(culture: CultureName.PT_BR, uiCulture: CultureName.PT_BR);
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;

                    options.RequestCultureProviders = new[] { new QueryStringRequestCultureProvider() };
                });

            services.AddHealthChecks();

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseRequestLocalization();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseMiddleware<SessionIdMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });

            return app;
        }
    }
}