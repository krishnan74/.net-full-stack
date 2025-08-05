using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DotnetAPI.DTOs.ShoppingCart;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO dto)
        {
            var result = await _shoppingCartService.AddToCartAsync(dto);
            return Ok(ApiResponse<object>.SuccessResponse(result, "Item added to cart"));
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateCart([FromBody] UpdateCartDTO dto)
        {
            var result = await _shoppingCartService.UpdateCartAsync(dto);
            return Ok(ApiResponse<object>.SuccessResponse(result, "Cart updated"));
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDTO dto)
        {
            var result = await _shoppingCartService.CheckoutAsync(dto);
            return Ok(ApiResponse<object>.SuccessResponse(result, "Checkout successful"));
        }

    }
}
