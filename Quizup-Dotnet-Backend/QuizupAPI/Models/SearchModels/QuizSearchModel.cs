using System;

namespace QuizupAPI.Models.SearchModels
{
    public class QuizSearchModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? TeacherName { get; set; }
        public Range<DateTime>? CreatedAt { get; set; }
        public Range<DateTime>? DueDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class Range<T>
    {
        public T? Min { get; set; }
        public T? Max { get; set; }
                
    }
}