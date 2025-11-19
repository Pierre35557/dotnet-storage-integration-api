using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StorageIntegrationApi.Api.Filters
{
    public class SharePointHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            var headers = new[]
            {
                "x-tenant-id",
                "x-client-id",
                "x-client-secret",
                "x-drive-id"
            };

            foreach (var header in headers)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = header,
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema { Type = "string" },
                    Description = $"Required integration header: {header}"
                });
            }
        }
    }
}
