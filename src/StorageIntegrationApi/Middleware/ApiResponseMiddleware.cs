using StorageIntegrationApi.Api.Models;
using StorageIntegrationApi.Application.Exceptions;

namespace StorageIntegrationApi.Api.Middleware
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiResponseMiddleware> _logger;

        private const string ApiKeyHeaderName = "x-api-key";
        private const string ClientIdHeaderName = "x-system-client-id";

        public ApiResponseMiddleware(RequestDelegate next, ILogger<ApiResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Bypass swagger/openapi/scalar docs
            if (context.Request.Path.StartsWithSegments("/openapi") ||
                context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/scalar"))
            {
                await _next(context);
                return;
            }

            try
            {
                ValidateClient(context);
                await _next(context);
            }
            catch (Exception ex)
            {
                var method = context.Request.Method;
                var path = context.Request.Path;

                _logger.LogError(ex,
                    "System exception {ExceptionType} caught in ApiResponseMiddleware [{Method} {Path}] - {Message}",
                    ex.GetType().Name, method, path, ex.Message);

                var (statusCode, message, errors) = MapExceptionToResponse(ex);

                var response = new ApiErrorResponse
                {
                    Success = false,
                    Message = message,
                    StatusCode = statusCode,
                    Errors = errors?.ToList()
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private static void ValidateClient(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ClientIdHeaderName, out var clientId) ||
                string.IsNullOrWhiteSpace(clientId))
            {
                throw new UnauthorizedAccessException("x-system-client-id header is missing.");
            }

            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedKey) ||
                string.IsNullOrWhiteSpace(providedKey))
            {
                throw new UnauthorizedAccessException("x-api-key header is missing.");
            }

            var normalizedClientId = clientId.ToString().Trim().ToUpper();
            var expectedKey = Environment.GetEnvironmentVariable($"CLIENT_API_KEY_{normalizedClientId}");

            if (string.IsNullOrWhiteSpace(expectedKey))
                throw new UnauthorizedAccessException("No API key configured for this client.");

            if (!string.Equals(providedKey.ToString(), expectedKey, StringComparison.Ordinal))
                throw new UnauthorizedAccessException("Invalid API key for this client.");

            context.Items["ClientId"] = normalizedClientId;
        }

        private static (int statusCode, string message, IEnumerable<string>? errors) MapExceptionToResponse(Exception ex)
        {
            return ex switch
            {
                UnauthorizedAccessException => (
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized Access.",
                    new[] { ex.Message }
                ),

                InvalidOperationException => (
                    StatusCodes.Status400BadRequest,
                    "The requested operation is invalid.",
                    new[] { ex.Message }
                ),

                ArgumentNullException => (
                    StatusCodes.Status400BadRequest,
                    "A required value was missing.",
                    new[] { ex.Message }
                ),

                ArgumentException => (
                    StatusCodes.Status400BadRequest,
                    "One or more arguments are invalid.",
                    new[] { ex.Message }
                ),

                BadRequestException vex => (
                    StatusCodes.Status400BadRequest,
                    "Validation failed for one or more fields.",
                    vex.Errors
                ),

                MissingHeadersException mhe => (
                    StatusCodes.Status400BadRequest,
                    "Required request headers are missing.",
                    mhe.MissingHeaders
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later.",
                    null
                )
            };
        }
    }
}
