using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetAPI.DTOs.Color;
using DotnetAPI.Models;
using DotnetAPI.Services;
using DotnetAPI.Interfaces;
using DotnetAPI.DTOs.Response;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ColorsController : ControllerBase
    {
        private readonly IColorService _colorService;
        public ColorsController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Color>>), 200)]
        public async Task<IActionResult> GetAllColors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var colors = await _colorService.GetAllColorsAsync(pageNumber, pageSize);
                return Ok(ApiResponse<IEnumerable<Color>>.SuccessResponse(colors, "Colors fetched successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching colors", ex.Message));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Color>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetColorById(int id)
        {
            try
            {
                var color = await _colorService.GetColorByIdAsync(id);
                return Ok(ApiResponse<Color>.SuccessResponse(color, "Color fetched successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching the color", ex.Message));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Color>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> AddColor([FromBody] AddColorDTO colorDTO)
        {
            try
            {
                var color = await _colorService.AddColorAsync(colorDTO);
                return CreatedAtAction(nameof(GetColorById), new { id = color.ColorId }, ApiResponse<Color>.SuccessResponse(color, "Color created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the color", ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Color>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> UpdateColor(int id, [FromBody] UpdateColorDTO colorDTO)
        {
            try
            {
                var color = await _colorService.UpdateColorAsync(id, colorDTO);
                return Ok(ApiResponse<Color>.SuccessResponse(color, "Color updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the color", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Color>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> DeleteColor(int id)
        {
            try
            {
                var color = await _colorService.DeleteColorAsync(id);
                return Ok(ApiResponse<Color>.SuccessResponse(color, "Color deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the color", ex.Message));
            }
        }
    }
}
