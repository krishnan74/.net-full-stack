namespace QuizupAPI.Models.DTOs.Authentication
{
    public class UserLoginResponseDTO
    {
        public long? UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? ClassGroupName { get; set; }
    }
}