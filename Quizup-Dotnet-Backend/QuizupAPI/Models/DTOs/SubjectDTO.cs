using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models.DTOs.Subject
{
    public class SubjectDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }
    }
}