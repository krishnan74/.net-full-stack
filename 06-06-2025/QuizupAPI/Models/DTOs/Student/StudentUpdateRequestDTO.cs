using System.ComponentModel.DataAnnotations;
namespace QuizupAPI.Models.DTOs.Student
{
    public class StudentUpdateRequestDTO
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;
        public long? ClassId { get; set; }
    }
}