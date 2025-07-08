using System.ComponentModel.DataAnnotations;
namespace QuizupAPI.Models.DTOs.Teacher
{
    public class TeacherUpdateRequestDTO
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;
        public long SubjectId { get; set; } = 0;
        public long ClassGroupId { get; set; } = 0;
        public long ClassId { get; set; } = 0;
        
    }
}