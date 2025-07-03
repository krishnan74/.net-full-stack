using QuizupAPI.Models;
namespace QuizupAPI.Models
{
    public class ClassSubject
    {
        public long Id { get; set; }
        public long ClassId { get; set; }
        public Class? Class { get; set; }
        
        public long SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}

