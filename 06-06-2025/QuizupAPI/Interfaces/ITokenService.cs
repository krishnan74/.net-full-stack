using QuizupAPI.Models;

namespace QuizupAPI.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}