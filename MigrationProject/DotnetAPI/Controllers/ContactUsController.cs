using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using DotnetAPI.DTOs.Response;
using DotnetAPI.DTOs.ContactUs;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService _contactUsService;
        public ContactUsController(IContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ContactUs>>), 200)]
        public async Task<IActionResult> GetAllContactUs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var contacts = await _contactUsService.GetAllContactUsAsync(pageNumber, pageSize);
                return Ok(ApiResponse<IEnumerable<ContactUs>>.SuccessResponse(contacts, "Contacts fetched successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching contacts", ex.Message));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ContactUs>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<IActionResult> GetContactUsById(int id)
        {
            try
            {
                var contact = await _contactUsService.GetContactUsByIdAsync(id);
                return Ok(ApiResponse<ContactUs>.SuccessResponse(contact, "Contact fetched successfully"));
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
        [ProducesResponseType(typeof(ApiResponse<ContactUs>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> AddContactUs([FromBody] AddContactUsDTO contactDTO)
        {
            try
            {
                var contact = await _contactUsService.AddContactUsAsync(contactDTO);
                return CreatedAtAction(nameof(GetContactUsById), new { id = contact.Id }, ApiResponse<ContactUs>.SuccessResponse(contact, "Contact created successfully"));
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
        [ProducesResponseType(typeof(ApiResponse<ContactUs>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> UpdateContactUs(int id, [FromBody] UpdateContactUsDTO contactDTO)
        {
            try
            {
                var contact = await _contactUsService.UpdateContactUsAsync(id, contactDTO);
                return Ok(ApiResponse<ContactUs>.SuccessResponse(contact, "Contact updated successfully"));
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
        [ProducesResponseType(typeof(ApiResponse<ContactUs>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> DeleteContactUs(int id)
        {
            try
            {
                var contact = await _contactUsService.DeleteContactUsAsync(id);
                return Ok(ApiResponse<ContactUs>.SuccessResponse(contact, "Contact deleted successfully"));
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
