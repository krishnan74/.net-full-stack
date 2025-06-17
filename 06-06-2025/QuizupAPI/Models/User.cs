using System.ComponentModel.DataAnnotations;
using QuizupAPI.Interfaces;

namespace QuizupAPI.Models
{
    public class User: ISoftDeletable
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public Teacher? Teacher { get; set; }
        public Student? Student { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

    }
}