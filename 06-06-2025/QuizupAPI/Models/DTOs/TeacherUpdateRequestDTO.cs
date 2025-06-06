using System.ComponentModel.DataAnnotations;
namespace QuizupAPI.Models.DTOs.Teacher
{
    public class TeacherUpdateRequestDTO
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required")]
        [MaxLength(100, ErrorMessage = "Subject cannot exceed 100 characters")]
        public string Subject { get; set; } = string.Empty;
    }
}