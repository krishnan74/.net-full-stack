//create model for QuizSubmissionDTO
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models.DTOs.Classe
{
    public class ClassUpdateDTO
    {
        [Required]
        public string ClassName { get; set; }
        public ICollection<long> AddSubjectIds { get; set; }
        public ICollection<long> RemoveSubjectIds { get; set; }

    }
}