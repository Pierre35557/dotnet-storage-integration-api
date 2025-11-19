using Microsoft.AspNetCore.Mvc;
using SharePointIntegrationApi.Api.Factories;
using SharePointIntegrationApi.Api.Models;
using SharePointIntegrationApi.Application.Dtos;
using SharePointIntegrationApi.Application.Interfaces;

namespace SharePointIntegrationApi.Api.Controllers
{
    /// <summary>
    /// Provides SharePoint folder management operations such as directory creation.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/storage")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageController"/> class.
        /// </summary>
        /// <param name="service">The SharePoint storage service used for interacting with SharePoint folders.</param>
        public StorageController(IStorageService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates a new directory within a SharePoint document library.
        /// </summary>
        /// <param name="request">The folder creation request containing the parent path and the new folder name.</param>
        /// <returns>The directory URL otherwise, an error response.</returns>
        /// <remarks>
        /// The caller must provide SharePoint authentication details via HTTP headers.
        /// </remarks>
        /// <response code="200">Returns the URL of the newly created directory.</response>
        /// <response code="default">Returns an <see cref="ApiErrorResponse"/> if directory creation fails.</response>
        [HttpPost("directories")]
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
