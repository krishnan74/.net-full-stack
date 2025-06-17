using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuizupAPI.Models.DTOs.Question;

namespace QuizupAPI.Models.DTOs.Quiz
{
    public class QuizUpdateRequestDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
    }
}