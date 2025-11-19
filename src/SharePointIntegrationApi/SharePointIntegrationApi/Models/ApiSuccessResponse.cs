namespace SharePointIntegrationApi.Api.Models
{
    public class ApiSuccessResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }
    }
}
