namespace QuizupAPI.Models
{
    public class Answer
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public Question? Question { get; set; }
        public long QuizSubmissionId { get; set; }
        public QuizSubmission? QuizSubmission { get; set; }
        public string SelectedAnswer { get; set; } = string.Empty;
    }
}