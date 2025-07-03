using QuizupAPI.Models;
using QuizupAPI.Interfaces;

namespace QuizupAPI.Models
{
    public class Class: ISoftDeletable
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Student>? Students { get; set; }
        public ICollection<TeacherClass>? Teachers { get; set; }
        public ICollection<Quiz>? Quizzes { get; set; }
        public ICollection<ClassSubject>? ClassSubjects { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}