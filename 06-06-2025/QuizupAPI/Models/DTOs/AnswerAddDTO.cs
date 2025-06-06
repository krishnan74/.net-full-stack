using System.ComponentModel.DataAnnotations;


namespace QuizupAPI.Models.DTOs.Answer
{
    public class AnswerAddDTO
    {
        public int QuestionId { get; set; }
        public int QuizSubmissionId { get; set; }
        
        [Required(ErrorMessage = "Selected answer is required.")]
        public string SelectedAnswer { get; set; } = string.Empty;

    }
}