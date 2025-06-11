namespace QuizupAPI.Models
{
    public class QuizSubmission
    {
        public long Id { get; set; }
        public long QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public long StudentId { get; set; }
        public Student? Student { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public List<Answer>? Answers { get; set; }
        public string SubmissionStatus { get; set; } = "InProgress";
        public long? Score { get; set; }
    }
}