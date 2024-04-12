using app.Domain.Adapter;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace app.ExcelAdapter.Microsoft.Extensions.DependencyInjection
{
    public static class ExcelAdapterCollectionExtensions
    {
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddExcelAdapter(
            this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IExcelAdapter, ExcelAdapter>();

            return services;
        }
    }
}