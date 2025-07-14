using AzureKeyVaultDemo.Models;
using AzureKeyVaultDemo.Interfaces;
using AzureKeyVaultDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace AzureKeyVaultDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _uploadService;

        public FileController(IFileService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> PostSingleFile([FromForm] FileUploadModel request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file to upload");

            try
            {
                await _uploadService.PostFileAsync(request.File);
                return Ok("File uploaded successfully");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("upload/multiple")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> PostMultipleFile([FromForm] List<IFormFile> fileDetails)
        {
            if (fileDetails == null || fileDetails.Count == 0)
            {
                return BadRequest("No files to upload");
            }

            try
            {
                await _uploadService.PostMultiFileAsync(fileDetails);
                return Ok("Files uploaded successfully");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest();
            }

            try
            {
                var fileStream = await _uploadService.DownloadFile(fileName);
                return File(fileStream, "application/octet-stream", fileName);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}