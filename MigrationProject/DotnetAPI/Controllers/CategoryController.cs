using Microsoft.AspNetCore.Mvc;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Models.DTOs.Response;
using ChienVHShopOnline.Models.DTOs.Category;

namespace ChienVHShopOnline.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategorysController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategorysController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of categories</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Category>>), 200)]
        public async Task<IActionResult> GetAllCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync(pageNumber, pageSize);
                return Ok(ApiResponse<IEnumerable<Category>>.SuccessResponse(categories, "Categories fetched successfully"));
            }

            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching categories", ex.Message));
            }
        }

        /// <summary>
        /// Get a category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category information</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Category>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return Ok(ApiResponse<Category>.SuccessResponse(category, "Category fetched successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching the category", ex.Message));
            }
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="categoryDto">Category information</param>
        /// <returns>Created category</returns>
        [HttpPost]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Category>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        // [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
                }



                var category = await _categoryService.AddCategoryAsync(categoryDto);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryId }, ApiResponse<Category>.SuccessResponse(category, "Category created successfully"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the category. " + ex.Message));
            }
        }

        /// <summary>
        /// Update category information
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="categoryDto">Updated category information</param>
        /// <returns>Updated category</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Category>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [ProducesResponseType(404)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO categoryDto)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
                }

                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryDto);

                return Ok(ApiResponse<Category>.SuccessResponse(updatedCategory, "Category updated successfully"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the category. " + ex.Message));
            }
        }

        /// <summary>
        /// Delete a category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Deleted category</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Category>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var deletedCategory = await _categoryService.DeleteCategoryAsync(id);

                return Ok(ApiResponse<Category>.SuccessResponse(deletedCategory, "Category deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the category. " + ex.Message));
            }
        }

        

    }
}