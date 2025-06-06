namespace QuizupAPI.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public ICollection<Answer>? Answers { get; set; }
    }
}