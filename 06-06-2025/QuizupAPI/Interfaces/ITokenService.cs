using QuizupAPI.Models;

namespace QuizupAPI.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
        public string GenerateRefreshToken();
        public Task<string> GenerateAndSaveRefreshTokenAsync(User user);
        public Task<User> ValidateRefreshTokenAsync(string username, string refreshToken);
    }
}