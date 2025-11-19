using Microsoft.OpenApi.Models;
using StorageIntegrationApi.Api.Filters;
using System.Reflection;

namespace StorageIntegrationApi.Api.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring Swagger/OpenAPI documentation.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Registers Swagger services and configures JWT bearer authentication in the documentation UI.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add Swagger to.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "StorageIntegrationApi.Api",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.OperationFilter<SharePointHeaderOperationFilter>();
            });

            return services;
        }
    }
}
