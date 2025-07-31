using DotnetAPI.Models;
using DotnetAPI.DTOs.Category;

namespace DotnetAPI.Misc.Mappers
{
    public class CategoryMapper
    {
        public Category? MapAddCategory(AddCategoryDTO addRequestDto)
        {
            Category category = new();
            category.Name = addRequestDto.Name;
            return category;
        }

        public Category? MapUpdateCategory(Category existingCategory, UpdateCategoryDTO updateRequestDto)
        {
            existingCategory.Name = updateRequestDto.Name;
            return existingCategory;
        }
    
    }
}