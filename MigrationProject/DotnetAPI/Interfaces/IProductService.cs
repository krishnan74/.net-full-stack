using DotnetAPI.Models;
using DotnetAPI.DTOs.Product;

namespace DotnetAPI.Interfaces
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(AddProductDTO product);
        Task<Product> UpdateProductAsync(int id, UpdateProductDTO product);
        Task<Product> DeleteProductAsync(int id);
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync(int pageNumber, int pageSize);
    }
}
