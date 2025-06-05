using Notify.Interfaces;
using AutoMapper;
using Notify.Models;
using Notify.Models.DTOs;
using Notify.Misc;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Notify.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<int, Employee> _employeeRepository;
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;

        public EmployeeService(
            IRepository<int, Employee> employeeRepository,
            IUserService userService,
            IEncryptionService encryptionService,
            ILogger<EmployeeService> logger,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _userService = userService;
            _encryptionService = encryptionService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Employee> CreateEmployeeAsync(EmployeeAddRequestDTO employeeDto)
        {
            try
            {
                var user = _mapper.Map<EmployeeAddRequestDTO, User>(employeeDto);

                var createdUser = await _userService.CreateUserAsync(user);
                var employee = employeeDto.ToEmployee(createdUser.Username);

                return await _employeeRepository.Add(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                throw;
            }
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            try
            {
                return await _employeeRepository.Get(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee by id {Id}", id);
                throw;
            }
        }

        public async Task<Employee?> GetEmployeeByNameAsync(string name)
        {
            try
            {
                var employees = await _employeeRepository.GetAll();
                return employees.FirstOrDefault(e => e.Name == name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee by name {Name}", name);
                throw;
            }
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                return await _employeeRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all employees");
                throw;
            }
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, EmployeeUpdateRequestDTO employeeDto)
        {
            try
            {
                var employee = await _employeeRepository.Get(id);
                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with id {id} not found");
                }

                employee.FirstName = employeeDto.FirstName;
                employee.LastName = employeeDto.LastName;
                employee.Department = employeeDto.Department;
                employee.Position = employeeDto.Position;

                return await _employeeRepository.Update(id, employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee {Id}", id);
                throw;
            }
        }

        public async Task<Employee> DeleteEmployeeAsync(int id)
        {
            try
            {
                return await _employeeRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee {Id}", id);
                throw;
            }
        }

        public async Task<Employee> GetEmployeeByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Employee name cannot be null or empty", nameof(name));

                var employees = await _employeeRepository.GetAll();
                var employee = employees.FirstOrDefault(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                return employee;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        

        public async Task<ICollection<EmployeesByDepartmentResponseDTO>> GetEmployeesByDepartment(string department)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(department))
                {
                    throw new ArgumentException("Department cannot be null or empty", nameof(department));
                }

                var result = await _employeeRepository.GetAll();

                result = result.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase)).ToList();

                var dto_result = _mapper.Map<IEnumerable<Employee>, ICollection<EmployeesByDepartmentResponseDTO>>(result);

                if (result == null || !result.Any())
                    return new List<EmployeesByDepartmentResponseDTO>();

                return dto_result;

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<EmployeesByDepartmentResponseDTO>();
            }

        }

    }
}