using StorageIntegrationApi.Application.Dtos;

namespace StorageIntegrationApi.Application.Interfaces
{
    public interface ISharePointStorageService
    {
        Task<string> GetOrCreateDirectoryAsync(SharePointConfig config, CreateFolderRequest request);
    }
}
