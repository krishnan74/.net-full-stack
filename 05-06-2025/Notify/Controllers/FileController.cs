using Notify.Models;
using Notify.Interfaces;
using Notify.Services;
using Notify.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Notify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IHubContext<DocumentHub> _hubContext;
        private readonly ILogger<FileController> _logger;

        public FileController(
            IFileService fileService,
            IHubContext<DocumentHub> hubContext,
            ILogger<FileController> logger)
        {
            _fileService = fileService;
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "HRAdmin,Admin")]
        public async Task<ActionResult<Document>> UploadDocument([FromForm] FileUploadModel fileDetails)
        {
            if (fileDetails == null)
            {
                return BadRequest("No file provided");
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var document = await _fileService.UploadDocumentAsync(fileDetails.FileDetails, fileDetails.FileType, userId);
                
                await _hubContext.Clients.All.SendAsync("ReceiveDocumentNotification", new
                {
                    Title = document.Title,
                    UploadedBy = User.Identity?.Name,
                    UploadedAt = document.UploadedAt
                });

                return Ok(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document");
                return StatusCode(500, "An error occurred while uploading the document");
            }
        }

        [HttpGet("documents")]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var documents = await _fileService.GetAccessibleDocumentsAsync(userId);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving documents");
                return StatusCode(500, "An error occurred while retrieving documents");
            }
        }

        [HttpGet("download/{id}")]
        public async Task<ActionResult> DownloadDocument(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var (fileStream, fileName, contentType) = await _fileService.DownloadDocumentAsync(id, userId);
                return File(fileStream, contentType, fileName);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading document {DocumentId}", id);
                return StatusCode(500, "An error occurred while downloading the document");
            }
        }

        [HttpPost("access")]
        [Authorize(Roles = "HRAdmin,Admin")]
        public async Task<ActionResult> GrantDocumentAccess(int documentId, int userId, bool canView = true, bool canDownload = true)
        {
            try
            {
                var grantedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(grantedBy))
                {
                    return Unauthorized();
                }

                await _fileService.GrantDocumentAccessAsync(documentId, userId, grantedBy, canView, canDownload);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error granting document access");
                return StatusCode(500, "An error occurred while granting document access");
            }
        }
    }
}