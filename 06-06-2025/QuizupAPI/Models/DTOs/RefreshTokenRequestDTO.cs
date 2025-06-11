namespace QuizupAPI.Models.DTOs.Authentication
{
    public class RefreshTokenRequestDTO
    {
        public string Username { get; set; } = string.Empty;
        public string RefreshToken { get; set; } 
    }
}