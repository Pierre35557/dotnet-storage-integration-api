using StorageIntegrationApi.Application.Dtos;

namespace StorageIntegrationApi.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string?> CreateFolderAsync(SharePointConfig config, CreateFolderRequest request);
    }
}
