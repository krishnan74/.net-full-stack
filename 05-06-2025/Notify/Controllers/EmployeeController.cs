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
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "HRAdmin")]
    public async Task<ActionResult<EmployeeResponseDTO>> CreateEmployee([FromBody] EmployeeAddRequestDTO employeeDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _employeeService.CreateEmployeeAsync(employeeDto);
            if (employee == null)
                return BadRequest("Failed to create employee");

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee.ToResponseDTO());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            return StatusCode(500, "An error occurred while creating the employee");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeResponseDTO>> GetEmployeeById(int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound($"Employee with ID {id} not found");

            return Ok(employee.ToResponseDTO());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the employee");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<EmployeeResponseDTO>> GetEmployeeByName(string name)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeByName(name);
            if (employee == null)
                return NotFound($"Employee with name {name} not found");

            return Ok(employee.ToResponseDTO());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee by name {Name}", name);
            return StatusCode(500, "An error occurred while retrieving the employee");
        }
    }

    

    [HttpGet("department/{department}")]
    [Authorize(Roles = "HRAdmin")]
    public async Task<ActionResult<ICollection<EmployeesByDepartmentResponseDTO>>> GetEmployeesByDepartment(string department)
    {
        try
        {
            var employees = await _employeeService.GetEmployeesByDepartment(department);
            return Ok(employees);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees by department {Department}", department);
            return StatusCode(500, "An error occurred while retrieving employees by department");
        }
    }
} 