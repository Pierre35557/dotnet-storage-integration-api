using Microsoft.Extensions.DependencyInjection;
using StorageIntegrationApi.Application.Interfaces;
using StorageIntegrationApi.Application.Services;

namespace StorageIntegrationApi.Application.ServiceRegistration
{
    /// <summary>
    /// Provides extension methods for registering application-layer services.
    /// </summary>
    public static class ApplicationServiceRegistration
    {
        /// <summary>
        /// Registers application-layer services with the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to which the services will be added.</param>
        /// <returns>
        /// The same <see cref="IServiceCollection"/> instance, enabling method chaining.
        /// </returns>
        /// <remarks>
        /// This method adds application-level abstractions and service implementations
        /// used throughout the application layer. These services are registered with a
        /// scoped lifetime, creating one instance per request.
        /// </remarks>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();

            return services;
        }
    }
}
