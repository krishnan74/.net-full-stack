using System.Text.Json.Serialization;
using QuizupAPI.Models;
namespace QuizupAPI.Models{
    public class TeacherClass{
        public long Id { get; set; }
        public long TeacherId { get; set; }

        [JsonIgnore]
        public Teacher? Teacher { get; set; }
        public long ClassId { get; set; }
        public Classe? Classe { get; set; } 
    }
}