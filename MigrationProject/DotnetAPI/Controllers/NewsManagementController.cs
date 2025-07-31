using System.Text;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

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
