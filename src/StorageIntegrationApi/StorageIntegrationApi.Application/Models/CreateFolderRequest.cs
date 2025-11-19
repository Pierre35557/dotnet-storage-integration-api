namespace StorageIntegrationApi.Application.Dtos
{
    public record CreateFolderRequest
    {
        public required string RootDirectory { get; init; }
        public required string FolderName { get; init; }
    }
}
