using QuizupAPI.Models;
namespace QuizupAPI.Models
{
    public class ClassSubject
    {
        public long Id { get; set; }
        public long ClassId { get; set; }
        public Classe? Classe { get; set; }
        
        public long SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}

