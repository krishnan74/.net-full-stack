using System.Text;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportOrderListing()
        {
            var pdfBytes = await _orderService.ExportOrderListingPdfAsync();
            var fileName = $"OrderListing_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
        // ...existing code for CRUD endpoints if needed...
    }
}
