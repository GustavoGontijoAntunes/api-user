using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace app.WebApi.Configuration
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(
            this IServiceCollection services,
            EnvironmentConfiguration environment)
        {
            var serviceProvider = services.BuildServiceProvider();
            var provider = serviceProvider.GetService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var openApiInfo = new OpenApiInfo
                    {
                        Version = description.GroupName,
                        Title = "APP API",
                        Description = $"Environment: {environment.Name} | CommitHash: {environment.CommitHash} | Resources for APP process",
                        Contact = new OpenApiContact
                        {
                            Name = "App"
                        }
                    };
                    c.SwaggerDoc(description.GroupName, openApiInfo);
                }

                var openApiSecurityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                };

                c.AddSecurityDefinition("Bearer", openApiSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                var assemblyname = Assembly.GetExecutingAssembly().FullName.Split(",")[0];
                var filePath = Path.Combine(AppContext.BaseDirectory, $"{assemblyname}.xml");

                if (File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(
            this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });

            return app;
        }
    }
}