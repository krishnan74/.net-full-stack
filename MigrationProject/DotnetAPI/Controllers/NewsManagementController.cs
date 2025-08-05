using System.Text;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.DTOs.News;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewsManagementController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsManagementController(INewsService newsService)
        {
            _newsService = newsService;
        }

         [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<News>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetNewsById(int id)
        {
            try
            {
                var news = await _newsService.GetNewsByIdAsync(id);
                return Ok(ApiResponse<News>.SuccessResponse(news, "News fetched successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching the news", ex.Message));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<News>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddNews([FromForm] AddNewsDTO newsDTO)
        {
            try
            {

                if (!ModelState.IsValid) return BadRequest(ModelState);

                string? filePath = null;

                if (newsDTO.ImageFile != null && newsDTO.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/news-images");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newsDTO.ImageFile.FileName);
                    var fullPath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await newsDTO.ImageFile.CopyToAsync(stream);
                    }

                    filePath = "/news-images/" + fileName;
                }

                newsDTO.ImageFile = null;
                var news = await _newsService.AddNewsAsync(newsDTO, filePath);
                return CreatedAtAction(nameof(GetNewsById), new { id = news.NewsId }, ApiResponse<News>.SuccessResponse(news, "News created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the news", ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<News>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> UpdateNews(int id, [FromForm] UpdateNewsDTO newsDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                string? filePath = null;

                if (newsDTO.ImageFile != null && newsDTO.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/news-images");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newsDTO.ImageFile.FileName);
                    var fullPath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await newsDTO.ImageFile.CopyToAsync(stream);
                    }

                    filePath = "/news-images/" + fileName;
                }
                newsDTO.ImageFile = null;

                var news = await _newsService.UpdateNewsAsync(id, newsDTO, filePath);
                return Ok(ApiResponse<News>.SuccessResponse(news, "News updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the news", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<News>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> DeleteNews(int id)
        {
            try
            {
                var news = await _newsService.DeleteNewsAsync(id);
                return Ok(ApiResponse<News>.SuccessResponse(news, "News deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the news", ex.Message));
            }
        }

        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportContentToCSV()
        {
            var csv = await _newsService.ExportNewsToCSVAsync();
            var fileName = $"NewsListing_{DateTime.Now:yyyyMMddHHmmss}.csv";
            return File(Encoding.UTF8.GetBytes(csv), "text/csv", fileName);
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportContentToExcel()
        {
            var excel = await _newsService.ExportNewsToExcelAsync();
            var fileName = $"NewsListing_{DateTime.Now:yyyyMMddHHmmss}.xls";
            return File(Encoding.UTF8.GetBytes(excel), "application/vnd.ms-excel", fileName);
        }
        // ...existing code for CRUD endpoints if needed...
    }
}
