using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models.DTOs.Answer
{
    public class AnswerAddRequestDTO
    {
        public long QuestionId { get; set; }
        
        [Required(ErrorMessage = "Selected answer is required.")]
        public string SelectedAnswer { get; set; } = string.Empty;

    }
}