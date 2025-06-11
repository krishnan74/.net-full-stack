using QuizupAPI.Models;

namespace QuizupAPI.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
        public string GenerateRefreshToken();
        public Task<string> GenerateAndSaveRefreshTokenAsync(User user);
        public Task<User> ValidateRefreshTokenAsync(string username, string refreshToken);
    }
}