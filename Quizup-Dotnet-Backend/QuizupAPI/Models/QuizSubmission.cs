namespace QuizupAPI.Models
{
    public class QuizSubmission
    {
        public long Id { get; set; }
        public long QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public long StudentId { get; set; }
        public Student? Student { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? SavedDate { get; set; }
        public List<Answer>? Answers { get; set; }
        public string SubmissionStatus { get; set; } = "InProgress";
        public int? Score { get; set; }
    }
}