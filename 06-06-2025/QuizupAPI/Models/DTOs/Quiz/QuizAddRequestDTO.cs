using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuizupAPI.Models.DTOs.Question;

namespace QuizupAPI.Models.DTOs.Quiz
{
    public class QuizAddRequestDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public ICollection<string>? Tags { get; set; }

        [Required]
        public long TeacherId { get; set; }

        [Required]
        public long ClassId { get; set; }

        [Required]
        public long SubjectId { get; set; }

        [Required]
        public List<QuestionAddRequestDTO> Questions { get; set; } = new List<QuestionAddRequestDTO>();
    }
}