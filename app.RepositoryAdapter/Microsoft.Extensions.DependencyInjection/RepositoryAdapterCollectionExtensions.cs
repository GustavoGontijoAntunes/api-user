using app.Domain.Repositories;
using app.RepositoryAdapter;
using app.RepositoryAdapter.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryAdapterCollectionExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddRepositoryAdapter(
            this IServiceCollection services,
            RepositoryAdapterConfiguration adapterConfiguration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (adapterConfiguration is null)
            {
                throw new ArgumentNullException(nameof(adapterConfiguration));
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(adapterConfiguration.ConnectionString, ServerVersion.AutoDetect(adapterConfiguration.ConnectionString),
                    config => config.MaxBatchSize(100));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                if (adapterConfiguration.EnableSensitiveDataLogging)
                {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }
            });

            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}