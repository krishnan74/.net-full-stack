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
        public int StudentId { get; set; }

        [Required]
        public int QuizId { get; set; }

        [Required]
        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        [Required]
        public List<AnswerAddDTO> Answers { get; set; } = new List<AnswerAddDTO>();
    }
}