using StorageIntegrationApi.Application.Dtos;
using StorageIntegrationApi.Application.Interfaces;

namespace StorageIntegrationApi.Application.Services
{
    /// <summary>
    /// Provides an application-level wrapper for storage folder operations.
    /// (e.g., SharePoint, Azure Blob Storage).
    /// </summary>
    public class StorageService : IStorageService
    {
        private readonly ISharePointStorageService _service;

        public StorageService(ISharePointStorageService service)
        {
            _service = service;
        }

        public async Task<string?> CreateFolderAsync(SharePointConfig config, CreateFolderRequest request)
            => await _service.GetOrCreateDirectoryAsync(config, request);
    }
}
