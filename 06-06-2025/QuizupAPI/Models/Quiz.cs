using QuizupAPI.Interfaces;

namespace QuizupAPI.Models
{
    public class Quiz : ISoftDeletable
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public ICollection<QuizQuestion>? QuizQuestions { get; set; }
        public ICollection<QuizSubmission>? Submissions { get; set; }
    }
}