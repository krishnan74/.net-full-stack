using QuizupAPI.Models;

namespace QuizupAPI.Models{
    public class TeacherSubject
    {
        public long Id { get; set; }
        public long TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public long SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}