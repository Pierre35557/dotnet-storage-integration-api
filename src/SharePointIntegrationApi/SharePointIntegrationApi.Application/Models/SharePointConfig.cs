using Microsoft.AspNetCore.Http;
using SharePointIntegrationApi.Application.Exceptions;

namespace SharePointIntegrationApi.Application.Dtos
{
    public class SharePointConfig
    {
        public string TenantId { get; set; } = default!;
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
        public string DriveId { get; set; } = default!;

        public static SharePointConfig FromHeaders(IHeaderDictionary headers)
        {
            return new SharePointConfig
            {
                TenantId = headers["x-tenant-id"],
                ClientId = headers["x-client-id"],
                ClientSecret = headers["x-client-secret"],
                DriveId = headers["x-drive-id"]
            };
        }

        public void Validate()
        {
            var missing = new List<string>();

            if (string.IsNullOrWhiteSpace(TenantId)) missing.Add("x-tenant-id");
            if (string.IsNullOrWhiteSpace(ClientId)) missing.Add("x-client-id");
            if (string.IsNullOrWhiteSpace(ClientSecret)) missing.Add("x-client-secret");
            if (string.IsNullOrWhiteSpace(DriveId)) missing.Add("x-drive-id");

            if (missing.Count > 0)
                throw new MissingHeadersException(missing);
        }
    }
}
