using System.ComponentModel.DataAnnotations;

namespace Notify.Models.DTOs
{
    public class EmployeeAddRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string Department { get; set; } = null!;

        public string Position { get; set; } = string.Empty;
        public DateTime? HireDate { get; set; }
    }
}