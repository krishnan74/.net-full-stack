using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models.DTOs.Teacher
{
    public class TeacherAddRequestDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        public ICollection<long>? ClassIds { get; set; } = new List<long>();
        public ICollection<long>? SubjectIds { get; set; } = new List<long>();
    }
}