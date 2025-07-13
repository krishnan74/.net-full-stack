using System;

namespace QuizupAPI.Models.SearchModels
{
    public class QuizSearchModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? TeacherName { get; set; }
        public DateTime? CreatedAtMin { get; set; }
        public DateTime? CreatedAtMax { get; set; }
        public DateTime? DueDateMin { get; set; }
        public DateTime? DueDateMax { get; set; }
        public bool IsActive { get; set; } = true;
        public string Role { get; set; } = string.Empty;
        public long? SearchId { get; set; }
    }

}