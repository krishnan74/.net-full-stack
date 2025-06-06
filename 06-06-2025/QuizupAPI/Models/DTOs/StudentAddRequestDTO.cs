using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models.DTOs.Student
{
    public class StudentAddRequestDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Class is required")]
        [MaxLength(100, ErrorMessage = "Class cannot exceed 100 characters")]
        public string Class { get; set; } = string.Empty;
    }
}