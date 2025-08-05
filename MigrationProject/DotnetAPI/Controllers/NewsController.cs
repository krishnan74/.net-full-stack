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

    }
}
