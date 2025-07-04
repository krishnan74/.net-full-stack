using QuizupAPI.Models;
using System.Text.Json.Serialization;

namespace QuizupAPI.Models{
    public class TeacherSubject
    {
        public long Id { get; set; }
        public long TeacherId { get; set; }

        [JsonIgnore]
        public Teacher? Teacher { get; set; }
        public long SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}