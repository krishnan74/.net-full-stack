namespace QuizupAPI.Models.DTOs.Authentication
{
    public class UserLoginResponseDTO
    {
        public string Username { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}