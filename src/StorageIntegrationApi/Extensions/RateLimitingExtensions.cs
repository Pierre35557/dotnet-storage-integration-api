using StorageIntegrationApi.Api.Models;
using System.Threading.RateLimiting;

namespace StorageIntegrationApi.Api.Extensions
{
    public static class RateLimitingExtensions
    {
        private const int DefaultPermitLimit = 30;
        private const int DefaultHeavyPermitLimit = 10;

        public static IServiceCollection AddApiRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.Headers["Retry-After"] = "60";
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";

                    var response = new ApiErrorResponse
                    {
                        Success = false,
                        Message = "Rate limit exceeded",
                        StatusCode = StatusCodes.Status429TooManyRequests,
                        Errors =
                        [
                            "Too many requests in a short period. Please wait and try again."
                        ]
                    };

                    await context.HttpContext.Response.WriteAsJsonAsync(response, token);
                };

                options.AddPolicy("default", httpContext =>
                {
                    var clientId =
                        httpContext.Items["ClientId"]?.ToString()?.ToUpper()
                        ?? "UNKNOWN";

                    var clientSpecificLimit =
                        Environment.GetEnvironmentVariable($"CLIENT_LIMIT_{clientId}");

                    var defaultLimit =
                        Environment.GetEnvironmentVariable("CLIENT_LIMIT_DEFAULT");

                    var permitLimit =
                        int.TryParse(clientSpecificLimit ?? defaultLimit, out var parsed)
                            ? parsed
                            : DefaultPermitLimit;

                    return RateLimitPartition.GetFixedWindowLimiter(clientId, _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = permitLimit,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            AutoReplenishment = true
                        });
                });

                options.AddPolicy("heavy", httpContext =>
                {
                    var clientId =
                        httpContext.Items["ClientId"]?.ToString()?.ToUpper()
                        ?? "UNKNOWN";

                    var clientSpecificLimit =
                        Environment.GetEnvironmentVariable($"CLIENT_LIMIT_{clientId}_HEAVY");

                    var defaultHeavyLimit =
                        Environment.GetEnvironmentVariable("CLIENT_LIMIT_DEFAULT_HEAVY");

                    var permitLimit =
                        int.TryParse(clientSpecificLimit ?? defaultHeavyLimit, out var parsed)
                            ? parsed
                            : DefaultHeavyPermitLimit;

                    return RateLimitPartition.GetFixedWindowLimiter(clientId, _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = permitLimit,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            AutoReplenishment = true
                        });
                });
            });

            return services;
        }
    }
}
