using Microsoft.Extensions.DependencyInjection;
using StorageIntegrationApi.Application.Interfaces;
using StorageIntegrationApi.Infrastructure.FileUpload;

namespace StorageIntegrationApi.Infrastructure.ServiceRegistration
{
    /// <summary>
    /// Provides extension methods for registering infrastructure-layer services.
    /// </summary>
    public static class InfrastructureServiceRegistration
    {
        /// <summary>
        /// Registers infrastructure-layer services with the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to which the services will be added.</param>
        /// <returns>
        /// The same <see cref="IServiceCollection"/> instance, enabling method chaining.
        /// </returns>
        /// <remarks>
        /// This method registers services responsible for external system integration,
        /// such as SharePoint storage operations. These services are registered with a
        /// scoped lifetime to ensure proper handling of per-request configurations.
        /// </remarks>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ISharePointStorageService, SharePointStorageService>();

            return services;
        }
    }
}
