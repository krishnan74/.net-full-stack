using System.ComponentModel.DataAnnotations;

namespace QuizupAPI.Models.DTOs.Authentication
{
    public class UserLoginRequestDTO
    {
        [Required(ErrorMessage = "Username is mandatory")]
        [MinLength(5,ErrorMessage ="Invalid entry for username")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is mandatory")]
        public string Password { get; set; } = string.Empty;
    }
}