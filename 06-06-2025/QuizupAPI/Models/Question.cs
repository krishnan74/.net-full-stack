namespace QuizupAPI.Models
{
    public class Question
    {
        public long Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public ICollection<QuizQuestion>? QuizQuestions { get; set; }
        public ICollection<Answer>? Answers { get; set; }
    }
}