using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StorageIntegrationApi.Api.Factories;
using StorageIntegrationApi.Api.Models;
using StorageIntegrationApi.Application.Dtos;
using StorageIntegrationApi.Application.Interfaces;

namespace StorageIntegrationApi.Api.Controllers
{
    /// <summary>
    /// Provides storage operations such as directory creation for supported providers
    /// (e.g., SharePoint, Azure, etc.).
    /// </summary>
    [ApiController]
    [Route("api/storage")]
    [Produces("application/json")]
    [EnableRateLimiting("default")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageController"/> class.
        /// </summary>
        /// <param name="service">The storage service used to interact with the configured backend provider.</param>
        public StorageController(IStorageService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new directory in the configured storage provider.
        /// </summary>
        /// <param name="request">
        /// The folder creation request containing the parent path and the new folder name.
        /// </param>
        /// <returns>
        /// A success response containing the created directory URL; otherwise an error response.
        /// </returns>
        /// <remarks>
        /// This endpoint relies on integration headers to determine the target storage provider
        /// and its access configuration.
        /// <para>
        /// For SharePoint, the following headers are required:
        /// <list type="bullet">
        ///     <item><term>x-tenant-id</term><description>The Azure AD tenant ID.</description></item>
        ///     <item><term>x-client-id</term><description>The Azure AD application (client) ID.</description></item>
        ///     <item><term>x-client-secret</term><description>The Azure AD application secret.</description></item>
        ///     <item><term>x-drive-id</term><description>The SharePoint document library drive ID.</description></item>
        /// </list>
        /// Support for additional storage providers (e.g., Azure Blob Storage) will follow similar patterns.
        /// </para>
        /// </remarks>
        /// <response code="200">Returns the URL of the newly created directory.</response>
        /// <response code="default">
        /// Returns an <see cref="ApiErrorResponse"/> if the operation fails.
        /// </response>
        [HttpPost("directories")]
        [EnableRateLimiting("heavy")]
        [Consumes("application/json")]
        [ProducesDefaultResponseType(typeof(ApiErrorResponse))]
        [ProducesResponseType(typeof(ApiSuccessResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateDirectory([FromBody] CreateFolderRequest request)
        {
            var config = HttpContext.Items["SharePointConfig"] as SharePointConfig;

            var result = await _service.CreateFolderAsync(config, request);

            return ApiResponseFactory.Ok(result);
        }
    }
}
