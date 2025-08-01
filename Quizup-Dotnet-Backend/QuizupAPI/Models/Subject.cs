using QuizupAPI.Models;
using QuizupAPI.Interfaces;

namespace QuizupAPI.Models{
    public class Subject: ISoftDeletable
    {
        public long Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ICollection<ClassSubject>? ClassSubjects { get; set; }
        public ICollection<TeacherSubject>? TeacherSubjects { get; set; }
        public ICollection<Quiz>? Quizzes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}