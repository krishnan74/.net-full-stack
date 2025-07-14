using System.ComponentModel.DataAnnotations;
namespace QuizupAPI.Models.DTOs.Teacher
{
    public class TeacherUpdateRequestDTO
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;
        public ICollection<long> AddSubjectIds { get; set; }
        public ICollection<long> RemoveSubjectIds { get; set; }
        public ICollection<long> AddClassIds { get; set; }
        public ICollection<long> RemoveClassIds { get; set; }
    }
}