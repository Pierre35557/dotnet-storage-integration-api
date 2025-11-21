using Microsoft.AspNetCore.Mvc.Filters;
using StorageIntegrationApi.Application.Exceptions;

namespace StorageIntegrationApi.Api.Filters
{
    public class BadRequestValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                throw new BadRequestException(errors);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
