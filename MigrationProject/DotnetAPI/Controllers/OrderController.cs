using System.Text;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.DTOs.Order;
using DotnetAPI.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var orders = await _orderService.GetAllOrdersAsync(pageNumber, pageSize);
            return Ok(ApiResponse<IEnumerable<Order>>.SuccessResponse(orders, "Orders fetched successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(ApiResponse<Order>.SuccessResponse(order, "Order fetched successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderDTO orderDTO)
        {
            var order = await _orderService.AddOrderAsync(orderDTO);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderID }, ApiResponse<Order>.SuccessResponse(order, "Order created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO orderDTO)
        {
            var order = await _orderService.UpdateOrderAsync(id, orderDTO);
            return Ok(ApiResponse<Order>.SuccessResponse(order, "Order updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.DeleteOrderAsync(id);
            return Ok(ApiResponse<Order>.SuccessResponse(order, "Order deleted successfully"));
        }

        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportOrderListing()
        {
            var pdfBytes = await _orderService.ExportOrderListingPdfAsync();
            var fileName = $"OrderListing_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}
