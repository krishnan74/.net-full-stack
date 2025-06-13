namespace QuizupAPI.Models
{
    public class DatabaseHealth
    {
        public string Status { get; set; } = string.Empty;
        public long ResponseTime { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Error { get; set; } 
    }
}