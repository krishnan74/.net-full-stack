//create model for QuizSubmissionDTO
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuizupAPI.Models.DTOs.Answer;

namespace QuizupAPI.Models.DTOs.QuizSubmission
{
    public class QuizSubmissionDTO
    {
        [Required]
        public long StudentId { get; set; }

        [Required]
        public long QuizId { get; set; }

        [Required]
        public List<AnswerAddRequestDTO> Answers { get; set; } = new List<AnswerAddRequestDTO>();
    }
}