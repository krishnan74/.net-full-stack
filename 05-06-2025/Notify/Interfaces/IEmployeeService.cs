using System.Collections.Generic;
using Notify.Models.DTOs;
using Notify.Models;

namespace Notify.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee?> CreateEmployeeAsync(EmployeeAddRequestDTO request);
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee?> GetEmployeeByName(string name);
        Task<ICollection<EmployeesByDepartmentResponseDTO>> GetEmployeesByDepartment(string department);
    }
}