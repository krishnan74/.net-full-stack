
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizupAPI.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Subject { get; set; } = string.Empty;
        public User? User { get; set; }
        public ICollection<Quiz>? Quizzes { get; set; }
    }
}