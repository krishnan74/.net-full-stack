using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.DTOs.Response;
using DotnetAPI.DTOs.News;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<News>>), 200)]
        public async Task<IActionResult> GetAllNews([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var news = await _newsService.GetAllNewsAsync(pageNumber, pageSize);
                return Ok(ApiResponse<IEnumerable<News>>.SuccessResponse(news, "News fetched successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching news", ex.Message));
            }
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
        public async Task<IActionResult> AddNews([FromBody] AddNewsDTO newsDTO)
        {
            try
            {
                var news = await _newsService.AddNewsAsync(newsDTO);
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
        public async Task<IActionResult> UpdateNews(int id, [FromBody] UpdateNewsDTO newsDTO)
        {
            try
            {
                var news = await _newsService.UpdateNewsAsync(id, newsDTO);
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
    }
}
