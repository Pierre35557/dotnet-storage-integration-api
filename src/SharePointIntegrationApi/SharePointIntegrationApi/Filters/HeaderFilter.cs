using Microsoft.AspNetCore.Mvc.Filters;
using SharePointIntegrationApi.Application.Dtos;
using SharePointIntegrationApi.Application.Exceptions;

namespace SharePointIntegrationApi.Api.Filters
{
    public class HeaderFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;

            var missing = new List<string>();

            var tenantId = headers["x-tenant-id"].ToString();
            var clientId = headers["x-client-id"].ToString();
            var clientSecret = headers["x-client-secret"].ToString();
            var driveId = headers["x-drive-id"].ToString();

            if (string.IsNullOrWhiteSpace(tenantId)) 
                missing.Add("x-tenant-id");
            if (string.IsNullOrWhiteSpace(clientId)) 
                missing.Add("x-client-id");
            if (string.IsNullOrWhiteSpace(clientSecret)) 
                missing.Add("x-client-secret");
            if (string.IsNullOrWhiteSpace(driveId)) 
                missing.Add("x-drive-id");

            if (missing.Count > 0)
                throw new MissingHeadersException(missing);

            var config = new SharePointConfig
            {
                TenantId = tenantId,
                ClientId = clientId,
                ClientSecret = clientSecret,
                DriveId = driveId
            };

            context.HttpContext.Items["SharePointConfig"] = config;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
