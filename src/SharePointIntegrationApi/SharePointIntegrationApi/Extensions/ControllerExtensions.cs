using Microsoft.AspNetCore.Mvc;
using SharePointIntegrationApi.Api.Filters;
using SharePointIntegrationApi.Api.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharePointIntegrationApi.Api.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring controller services and behavior.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Configures MVC controllers with JSON settings and model validation response behavior.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
        /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
        /// 
        public static IServiceCollection ConfigureApiControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
                options.Filters.Add<BadRequestValidationFilter>();
                options.Filters.Add<HeaderFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            services.ConfigureHttpJsonOptions(options =>
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
            );

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    var response = new ApiErrorResponse
                    {
                        Success = false,
                        Message = "Validation failed",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
    }
}
