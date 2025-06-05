namespace Notify.Models.DTOs
{
    public class HRAdminsByDepartmentResponseDTO
    {
        public string Department { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
    }
}