using System.Text.Json;

namespace QuizupAPI.Models.DTOs.Teacher
{
    public class TeacherSummaryDTO
    {
        public long TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string TeacherEmail { get; set; } = string.Empty;
        public string TeacherSubject { get; set; } = string.Empty;
        public int TotalQuizzesCreated { get; set; }
        public int TotalActiveQuizzes { get; set; }
        public int TotalInactiveQuizzes { get; set; }
        public int TotalQuestionsCreated { get; set; }
        public int TotalStudentSubmissions { get; set; }
        public int TotalStudentsParticipated { get; set; }
        public decimal AverageCompletionRate { get; set; }
        public decimal AverageStudentScore { get; set; }
        public int HighestQuizScore { get; set; }
        public int LowestQuizScore { get; set; }
        public int TotalQuestionsAnswered { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public decimal OverallAccuracyPercentage { get; set; }
        public JsonDocument QuizzesByStatus { get; set; } = JsonDocument.Parse("{}");
        public JsonDocument StudentPerformanceSummary { get; set; } = JsonDocument.Parse("[]");
        public JsonDocument RecentQuizActivity { get; set; } = JsonDocument.Parse("[]");
        public JsonDocument QuizPerformanceTrend { get; set; } = JsonDocument.Parse("[]");
    }
} 