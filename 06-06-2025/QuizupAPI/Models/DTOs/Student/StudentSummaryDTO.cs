using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models.DTOs.Student
{
    public class StudentSummaryDTO
    {
        public long StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string StudentClass { get; set; } = string.Empty;
        public long TotalQuizzesAvailable { get; set; }
        public long TotalQuizzesStarted { get; set; }
        public long TotalQuizzesCompleted { get; set; }
        public long TotalQuizzesInProgress { get; set; }
        public long TotalQuizzesSaved { get; set; }
        public decimal AverageScore { get; set; }
        public int HighestScore { get; set; }
        public int LowestScore { get; set; }
        public long TotalQuestionsAttempted { get; set; }
        public long TotalCorrectAnswers { get; set; }
        public decimal AccuracyPercentage { get; set; }
        public int TotalTimeSpentMinutes { get; set; }
        public JsonDocument QuizzesByStatus { get; set; } = JsonDocument.Parse("{}");
        public JsonDocument RecentActivity { get; set; } = JsonDocument.Parse("[]");
        public JsonDocument PerformanceTrend { get; set; } = JsonDocument.Parse("[]");
    }
} 