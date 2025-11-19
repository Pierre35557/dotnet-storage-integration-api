using SharePointIntegrationApi.Application.Dtos;

namespace SharePointIntegrationApi.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string?> CreateFolderAsync(SharePointConfig config, CreateFolderRequest request);
    }
}
