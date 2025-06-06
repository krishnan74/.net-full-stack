namespace QuizupAPI.Models
{
    public class QuizSubmission
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public List<Answer>? Answers { get; set; }
        public string SubmissionStatus { get; set; } = "InProgress";
        public int? Score { get; set; }
    }
}