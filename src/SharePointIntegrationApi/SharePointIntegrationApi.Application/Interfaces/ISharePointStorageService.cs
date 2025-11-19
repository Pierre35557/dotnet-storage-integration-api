using SharePointIntegrationApi.Application.Dtos;

namespace SharePointIntegrationApi.Application.Interfaces
{
    public interface ISharePointStorageService
    {
        Task<string> GetOrCreateDirectoryAsync(SharePointConfig config, CreateFolderRequest request);
    }
}
