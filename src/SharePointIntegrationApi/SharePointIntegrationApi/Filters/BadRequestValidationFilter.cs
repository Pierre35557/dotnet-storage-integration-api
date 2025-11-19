using Microsoft.AspNetCore.Mvc.Filters;
using SharePointIntegrationApi.Application.Exceptions;

namespace SharePointIntegrationApi.Api.Filters
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
