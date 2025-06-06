namespace QuizupAPI.Models.DTOs.Authentication
{
    public class UserLoginResponseDTO
    {
        public string Username { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}