using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public Teacher? Teacher { get; set; }
        public Student? Student { get; set; }
    }
}