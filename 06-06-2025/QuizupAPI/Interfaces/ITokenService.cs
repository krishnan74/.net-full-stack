using QuizupAPI.Models;

namespace QuizupAPI.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}