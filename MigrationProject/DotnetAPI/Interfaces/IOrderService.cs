using System.Threading.Tasks;
using DotnetAPI.Models;
using DotnetAPI.DTOs.Order;
using System.Collections.Generic;

namespace DotnetAPI.Interfaces
{
    public interface IOrderService
    {
        Task<byte[]> ExportOrderListingPdfAsync();
        Task<Order> AddOrderAsync(AddOrderDTO order);
        Task<Order> UpdateOrderAsync(int id, UpdateOrderDTO order);
        Task<Order> DeleteOrderAsync(int id);
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync(int pageNumber, int pageSize);
    }
}
