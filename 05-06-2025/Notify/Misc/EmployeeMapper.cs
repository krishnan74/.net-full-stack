using Notify.Models.DTOs;
using Notify.Models;

namespace Notify.Misc;

public static class EmployeeMapper
{
    public static Employee ToEmployee(this EmployeeAddRequestDTO dto, string userName)
    {
        return new Employee
        {
            Username = userName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Department = dto.Department,
            Position = dto.Position,
            HireDate = dto.HireDate ?? DateTime.MinValue
            
        };
    }

    public static EmployeeResponseDTO ToResponseDTO(this Employee employee)
    {
        return new EmployeeResponseDTO
        {
            Id = employee.Id,
            Username = employee.Username,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Department = employee.Department,
            Position = employee.Position,
            HireDate = employee.HireDate,
            Name = employee.Name
        };
    }
}