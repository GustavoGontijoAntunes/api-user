namespace app.WebApi.Configuration
{
    public static class ExtensionsConfiguration
    {
        public static T SafeGet<T>(this IConfiguration configuration)
        {
            var typeName = typeof(T).Name;

            if (configuration.GetChildren().Any(item => item.Key == typeName))
            {
                configuration = configuration.GetSection(typeName);
            }

            T model = configuration.Get<T>();

            if (model == null)
            {
                throw new InvalidOperationException(
                    $"Configuration item not found for type {typeof(T).FullName}.");
            }

            return model;
        }
    }
}