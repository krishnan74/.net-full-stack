using System.Text.Json;

namespace QuizupAPI.Models.DTOs.Student
{
    public class StudentSummaryDTO
    {
        public long StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string StudentClass { get; set; } = string.Empty;
        public int TotalQuizzesAvailable { get; set; }
        public int TotalQuizzesStarted { get; set; }
        public int TotalQuizzesCompleted { get; set; }
        public int TotalQuizzesInProgress { get; set; }
        public int TotalQuizzesSaved { get; set; }
        public decimal AverageScore { get; set; }
        public int HighestScore { get; set; }
        public int LowestScore { get; set; }
        public int TotalQuestionsAttempted { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public decimal AccuracyPercentage { get; set; }
        public int TotalTimeSpentMinutes { get; set; }
        public JsonDocument QuizzesByStatus { get; set; } = JsonDocument.Parse("{}");
        public JsonDocument RecentActivity { get; set; } = JsonDocument.Parse("[]");
        public JsonDocument PerformanceTrend { get; set; } = JsonDocument.Parse("[]");
    }
} 