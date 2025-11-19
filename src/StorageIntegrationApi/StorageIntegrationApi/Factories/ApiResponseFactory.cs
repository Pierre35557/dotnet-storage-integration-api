using Microsoft.AspNetCore.Mvc;
using StorageIntegrationApi.Api.Models;

namespace StorageIntegrationApi.Api.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult Ok<T>(T data, string message = "Record(s) successful fetched.")
        {
            return new OkObjectResult(new ApiSuccessResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = StatusCodes.Status200OK,
                Data = data
            });
        }

        public static IActionResult Created<T>(string actionName, string controllerName, object routeValues, T data, string message = "Record successfully created.")
        {
            return new CreatedAtActionResult(actionName, controllerName, routeValues, new ApiSuccessResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = StatusCodes.Status201Created,
                Data = data
            });
        }

        public static IActionResult NoContent()
        {
            return new NoContentResult();
        }

        public static IActionResult BadRequest(IEnumerable<string> errors, string message = "Invalid request data.")
        {
            return new BadRequestObjectResult(new ApiErrorResponse
            {
                Success = false,
                Message = message,
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = errors
            });
        }

        public static IActionResult Conflict(string message = "A conflict occurred.")
        {
            return new ConflictObjectResult(new ApiErrorResponse
            {
                Success = false,
                Message = message,
                StatusCode = StatusCodes.Status409Conflict
            });
        }

        public static IActionResult NotFound(string message = "Record not found.")
        {
            return new NotFoundObjectResult(new ApiErrorResponse
            {
                Success = false,
                Message = message,
                StatusCode = StatusCodes.Status404NotFound
            });
        }

        public static IActionResult Unauthorized(string message = "Unauthorized access.")
        {
            return new ObjectResult(new ApiErrorResponse
            {
                Success = false,
                Message = message,
                StatusCode = StatusCodes.Status401Unauthorized
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }

        public static IActionResult Forbidden(string message = "Forbidden access.")
        {
            return new ObjectResult(new ApiErrorResponse
            {
                Success = false,
                Message = message,
                StatusCode = StatusCodes.Status403Forbidden
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        public static IActionResult ServerError(string message = "An unexpected error occurred while processing your request.")
        {
            return new ObjectResult(new ApiErrorResponse
            {
                Success = false,
                Message = message,
                StatusCode = StatusCodes.Status500InternalServerError
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
