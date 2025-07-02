using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models.DTOs.Teacher
{
    public class TeacherSummaryDTO
    {
        public long TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string TeacherEmail { get; set; } = string.Empty;
        public JsonDocument TeacherSubjects { get; set; } = JsonDocument.Parse("[]");
        public long TotalQuizzesCreated { get; set; }
        public long TotalActiveQuizzes { get; set; }
        public long TotalInactiveQuizzes { get; set; }
        public long TotalQuestionsCreated { get; set; }
        public long TotalStudentSubmissions { get; set; }
        public long TotalStudentsParticipated { get; set; }
        public decimal AverageCompletionRate { get; set; }
        public decimal AverageStudentScore { get; set; }
        public int HighestQuizScore { get; set; }
        public int LowestQuizScore { get; set; }
        public long TotalQuestionsAnswered { get; set; }
        public long TotalCorrectAnswers { get; set; }
        public decimal OverallAccuracyPercentage { get; set; }
        public JsonDocument QuizzesByStatus { get; set; } = JsonDocument.Parse("{}");
        public JsonDocument StudentPerformanceSummary { get; set; } = JsonDocument.Parse("[]");
        public JsonDocument RecentQuizActivity { get; set; } = JsonDocument.Parse("[]");
        public JsonDocument QuizPerformanceTrend { get; set; } = JsonDocument.Parse("[]");
    }
}