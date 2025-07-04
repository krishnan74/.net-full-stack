namespace QuizupAPI.Models.DTOs.Notifications
{
    public class QuizNotificationDTO
    {
        public long Id { get; set; }
        public string TeacherName { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public string ClassName { get; set; }
        public string Title { get; set; }
        public string Type { get; set; } 
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? DueDate { get; set; }
    }
}