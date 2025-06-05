namespace Notify.Models.DTOs;

public class EmployeeResponseDTO
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public string Name { get; set; } = string.Empty;
} 