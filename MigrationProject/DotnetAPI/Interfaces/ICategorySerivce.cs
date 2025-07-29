using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.DTOs.Category;

namespace ChienVHShopOnline.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> AddCategoryAsync(AddCategoryDTO category);
        Task<Category> UpdateCategoryAsync(int id, UpdateCategoryDTO category);
        Task<Category> DeleteCategoryAsync(int id);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync(int pageNumber, int pageSize);
    }
}