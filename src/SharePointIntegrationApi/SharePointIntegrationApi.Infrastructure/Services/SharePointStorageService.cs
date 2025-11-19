using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using SharePointIntegrationApi.Application.Dtos;
using SharePointIntegrationApi.Application.Interfaces;

namespace SharePointIntegrationApi.Infrastructure.FileUpload
{
    /// <summary>
    /// Provides SharePoint folder operations such as retrieving or creating directories.
    /// </summary>
    public class SharePointStorageService : ISharePointStorageService
    {
        /// <summary>
        /// Retrieves an existing SharePoint folder or creates it if it does not exist.
        /// </summary>
        /// <param name="config">
        /// The SharePoint configuration containing tenant credentials and the target drive ID.
        /// </param>
        /// <param name="request">
        /// The folder creation request specifying the parent directory and folder name.
        /// </param>
        /// <returns>
        /// The URL of the retrieved or newly created folder, or <c>null</c> if no folder was returned.
        /// </returns>
        public async Task<string?> GetOrCreateDirectoryAsync(SharePointConfig config, CreateFolderRequest request)
        {
            var client = CreateClient(config);
            DriveItem? folder = null;

            try
            {
                folder = await client
                    .Drives[config.DriveId]
                    .Root
                    .ItemWithPath($"{request.RootDirectory}/{request.FolderName}")
                    .GetAsync();
            }
            catch (ODataError ex) when (ex.ResponseStatusCode == 404)
            {
                var folderToCreate = new DriveItem
                {
                    Name = request.FolderName,
                    Folder = new Folder(),
                    AdditionalData = new Dictionary<string, object>
                    {
                        {"@microsoft.graph.conflictBehavior", "fail"}
                    }
                };

                folder = await client
                    .Drives[config.DriveId]
                    .Root
                    .ItemWithPath(request.RootDirectory)
                    .Children
                    .PostAsync(folderToCreate);
            }
            catch (ODataError ex)
            {
                throw new Exception($"Unable to create directory: {ex.Message}");
            }

            return folder?.WebUrl;
        }

        private static GraphServiceClient CreateClient(SharePointConfig config)
        {
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var credential = new ClientSecretCredential(
                config.TenantId,
                config.ClientId,
                config.ClientSecret,
                options
            );

            return new GraphServiceClient(credential);
        }
    }
}
