using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notify.Models.DTOs;
using Notify.Services;
using Notify.Interfaces;
using Notify.Models;
using Notify.Misc;

namespace Notify.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "HRAdmin")]
public class HRAdminController : ControllerBase
{
    private readonly IHRAdminService _hrAdminService;
    private readonly ILogger<HRAdminController> _logger;

    public HRAdminController(IHRAdminService hrAdminService, ILogger<HRAdminController> logger)
    {
        _hrAdminService = hrAdminService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<HRAdminResponseDTO>> CreateHRAdmin([FromBody] HRAdminAddRequestDTO hrAdminDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hrAdmin = await _hrAdminService.CreateHRAdminAsync(hrAdminDto);
            if (hrAdmin == null)
                return BadRequest("Failed to create HR admin");

            return CreatedAtAction(nameof(GetHRAdminById), new { id = hrAdmin.Id }, hrAdmin.ToResponseDTO());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating HR admin");
            return StatusCode(500, "An error occurred while creating the HR admin");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HRAdminResponseDTO>> GetHRAdminById(int id)
    {
        try
        {
            var hrAdmin = await _hrAdminService.GetHRAdminByIdAsync(id);
            if (hrAdmin == null)
                return NotFound($"HR Admin with ID {id} not found");

            return Ok(hrAdmin.ToResponseDTO());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving HR admin {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the HR admin");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<HRAdminResponseDTO>> GetHRAdminByName(string name)
    {
        try
        {
            var hrAdmin = await _hrAdminService.GetHRAdminByName(name);
            if (hrAdmin == null)
                return NotFound($"HR Admin with name {name} not found");

            return Ok(hrAdmin.ToResponseDTO());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving HR admin by name {Name}", name);
            return StatusCode(500, "An error occurred while retrieving the HR admin");
        }
    }

    [HttpGet("department/{department}")]
    public async Task<ActionResult<ICollection<HRAdminsByDepartmentResponseDTO>>> GetHRAdminsByDepartment(string department)
    {
        try
        {
            var hrAdmins = await _hrAdminService.GetHRAdminsByDepartment(department);
            return Ok(hrAdmins);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving HR admins by department {Department}", department);
            return StatusCode(500, "An error occurred while retrieving HR admins by department");
        }
    }
} 