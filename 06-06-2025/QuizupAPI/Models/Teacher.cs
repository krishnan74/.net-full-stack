
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizupAPI.Models
{
    public class Teacher
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
        public string Subject { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public User? User { get; set; }
        public ICollection<Quiz>? Quizzes { get; set; }
    }
}