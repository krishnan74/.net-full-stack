//create model for QuizSubmissionDTO
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models.DTOs.Classe
{
    public class ClassDTO
    {
        [Required]
        public string ClassName { get; set; }

        public ICollection<long> SubjectIds { get; set; }
    }
}