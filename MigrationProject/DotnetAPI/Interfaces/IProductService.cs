using DotnetAPI.Models;
using DotnetAPI.DTOs.Product;

namespace DotnetAPI.Interfaces
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(AddProductDTO product, string filePath = null);
        Task<Product> UpdateProductAsync(int id, UpdateProductDTO product, string filePath = null);
        Task<Product> DeleteProductAsync(int id);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync(int pageNumber, int pageSize);
    }
}
