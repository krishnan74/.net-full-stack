using DotnetAPI.Models;
using DotnetAPI.DTOs.ShoppingCart;
using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DotnetAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<int, Cart> _cartRepository;
        private readonly IRepository<int, Order> _orderRepository;
        public ShoppingCartService(IRepository<int, Cart> cartRepository, IRepository<int, Order> orderRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
        }

        public async Task<object> AddToCartAsync(AddToCartDTO dto)
        {
            var cart = await GetByUserIdAsync(dto.UserId);
            if (cart == null)
            {
                cart = new Cart { UserId = dto.UserId };
                await _cartRepository.Add(cart);
            }
            var item = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (item == null)
            {
                cart.Items.Add(new CartItem { ProductId = dto.ProductId, Quantity = dto.Quantity });
            }
            else
            {
                item.Quantity += dto.Quantity;
            }
            await _cartRepository.Update(cart.CartId, cart);
            return cart;
        }

        public async Task<object> UpdateCartAsync(UpdateCartDTO dto)
        {
            var cart = await GetByUserIdAsync(dto.UserId);
            if (cart == null)
                return null;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (item != null)
            {
                item.Quantity = dto.Quantity;
            }
            await _cartRepository.Update(cart.CartId, cart);
            return cart;
        }

        public async Task<object> CheckoutAsync(CheckoutDTO dto)
        {
            var cart = await GetByUserIdAsync(dto.UserId);
            if (cart == null)
                return null;
            // Implement order creation logic here using cart.Items
            var order = new Order
            {
                OrderName = $"Order_{dto.UserId}_{System.DateTime.UtcNow:yyyyMMddHHmmss}",
                OrderDate = System.DateTime.UtcNow,
                PaymentType = "Cash",
                Status = "Processing",
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                CustomerEmail = dto.CustomerEmail,
                CustomerAddress = dto.CustomerAddress,
                OrderDetails = cart.Items.Select(i => new OrderDetail
                {
                    ProductID = i.ProductId,
                    Quantity = i.Quantity,
                    Price = null // Set price if needed
                }).ToList()
            };
            // Save order to database (assume you have an IOrderRepository injected, or use context)
            // If you want to use repository, inject IOrderRepository and call await _orderRepository.Add(order);
            // For now, use context directly for demonstration:
            await _orderRepository.Add(order);
            await _cartRepository.Delete(cart.CartId);
            return new { Success = true };
        }

        private async Task<Cart> GetByUserIdAsync(int userId)
        {
            var carts = await _cartRepository.GetAll();
            if (!carts.Any())
                return null;

            return carts.FirstOrDefault(c => c.UserId == userId);
        }
    }
}
