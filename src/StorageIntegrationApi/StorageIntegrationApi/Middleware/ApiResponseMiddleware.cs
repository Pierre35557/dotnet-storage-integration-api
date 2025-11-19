using StorageIntegrationApi.Api.Models;
using StorageIntegrationApi.Application.Exceptions;

namespace StorageIntegrationApi.Api.Middleware
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiResponseMiddleware> _logger;

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

        private static (int statusCode, string message, IEnumerable<string>? errors) MapExceptionToResponse(Exception ex)
        {
            return ex switch
            {
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
