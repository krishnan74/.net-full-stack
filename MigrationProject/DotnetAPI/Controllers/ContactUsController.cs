using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.DTOs.Response;
using DotnetAPI.DTOs.ContactU;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUService _contactUService;
        public ContactUsController(IContactUService contactUService)
        {
            _contactUService = contactUService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContactU>>), 200)]
        public async Task<IActionResult> GetAllContactUs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var contacts = await _contactUService.GetAllContactUsAsync(pageNumber, pageSize);
                return Ok(ApiResponse<IEnumerable<ContactU>>.SuccessResponse(contacts, "Contacts fetched successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching contacts", ex.Message));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ContactU>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetContactUById(int id)
        {
            try
            {
                var contact = await _contactUService.GetContactUByIdAsync(id);
                return Ok(ApiResponse<ContactU>.SuccessResponse(contact, "Contact fetched successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching the contact", ex.Message));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ContactU>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> AddContactU([FromBody] AddContactUDTO contactDTO)
        {
            try
            {
                var contact = await _contactUService.AddContactUAsync(contactDTO);
                return CreatedAtAction(nameof(GetContactUById), new { id = contact.id }, ApiResponse<ContactU>.SuccessResponse(contact, "Contact created successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the contact", ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ContactU>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> UpdateContactU(int id, [FromBody] UpdateContactUDTO contactDTO)
        {
            try
            {
                var contact = await _contactUService.UpdateContactUAsync(id, contactDTO);
                return Ok(ApiResponse<ContactU>.SuccessResponse(contact, "Contact updated successfully"));
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
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the contact", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ContactU>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> DeleteContactU(int id)
        {
            try
            {
                var contact = await _contactUService.DeleteContactUAsync(id);
                return Ok(ApiResponse<ContactU>.SuccessResponse(contact, "Contact deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the contact", ex.Message));
            }
        }
    }
}
