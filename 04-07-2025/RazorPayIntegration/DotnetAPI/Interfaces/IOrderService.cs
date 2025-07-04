using DotnetAPI.Models;
using DotnetAPI.Models.DTOs.Order;
namespace DotnetAPI.Interfaces
{
    public interface IOrderService
    {
        public Task<Order> CreateOrderAsync(OrderDTO orderDTO);
        public Task<IEnumerable<Order>> GetAllOrdersAsync();
        public Task<Order> GetOrderByIdAsync(Guid id);
    }
}