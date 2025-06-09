using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models
{
    public class Student
    {
        public long Id { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Class { get; set; } = string.Empty;

        public User? User { get; set; }
        
        public ICollection<QuizSubmission>? QuizSubmissions { get; set; }
    }
}