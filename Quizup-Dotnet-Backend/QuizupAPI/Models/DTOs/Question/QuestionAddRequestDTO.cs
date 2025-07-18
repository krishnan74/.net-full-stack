using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models.DTOs.Question
{
    public class QuestionAddRequestDTO
    {
        public long? Id { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Question text cannot be longer than 500 characters.")]
        public string Text { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();

        [Required]
        public string CorrectAnswer { get; set; } = string.Empty;
    }
}