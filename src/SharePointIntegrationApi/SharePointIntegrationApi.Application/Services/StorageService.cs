using SharePointIntegrationApi.Application.Dtos;
using SharePointIntegrationApi.Application.Interfaces;

namespace SharePointIntegrationApi.Application.Services
{
    /// <summary>
    /// Provides an application-level wrapper for SharePoint folder operations.
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
