namespace QuizupAPI.Models
{
    public class Quiz
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Question>? Questions { get; set; }
        public ICollection<QuizSubmission>? Submissions { get; set; }
    }
}