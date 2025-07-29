using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Contexts;
using ChienVHShopOnline.Models.DTOs.Category;
using ChienVHShopOnline.Misc.Mappers;

namespace ChienVHShopOnline.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<int, Category> _categoryRepository;

        public CategoryMapper categoryMapper;
        private readonly ChienVHShopOnlineContext _context;
        public CategoryService(IRepository<int, Category> categoryRepository, ChienVHShopOnlineContext context)
        {
            _categoryRepository = categoryRepository;
            categoryMapper = new CategoryMapper();
            _context = context;
        }

        public async Task<Category> AddCategoryAsync(AddCategoryDTO categoryDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryDTO.Name))
                {
                    throw new ArgumentException("Category name cannot be null or empty.");
                }

                var newCategory = categoryMapper.MapAddCategory(categoryDTO);

                if (newCategory == null)
                {
                    throw new Exception("Failed to map CategoryDTO to Category.");
                }

                var addedCategory = await _categoryRepository.Add(newCategory);

                if (addedCategory == null)
                {
                    throw new Exception("Failed to add new category.");
                }

                return addedCategory;

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid category data: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the category.", ex);
            }
        }

        public async Task<Category> UpdateCategoryAsync(int id, UpdateCategoryDTO categoryDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryDTO.Name))
                {
                    throw new ArgumentException("Category name cannot be null or empty.");
                }
                var existingCategory = await _categoryRepository.Get(id);

                var mappedCategory = categoryMapper.MapUpdateCategory(existingCategory, categoryDTO);
                if (mappedCategory == null)
                {
                    throw new Exception("Failed to map CategoryDTO to existing Category.");
                }

                var updatedCategory = await _categoryRepository.Update(id, mappedCategory);

                if (updatedCategory == null)
                {
                    throw new Exception("Failed to update category.");
                }

                return updatedCategory;

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Invalid category data: {ex.Message}", ex);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category.", ex);
            }
        }

        public async Task<Category> DeleteCategoryAsync(int id)
        {
            try
            {
                var existingCategory = await _categoryRepository.Get(id);

                var deletedCategory = await _categoryRepository.Delete(id);

                return deletedCategory;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"No category found with ID: {id}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the category with ID {id}.", ex);
            }
            
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            try
            {
                return await _categoryRepository.Get(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the category with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(
            int pageNumber,
            int pageSize
        )
        {
             try
            {
                return await _categoryRepository.GetAll(pageNumber, pageSize);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all categories.", ex);
            }
        }
    
    }
}