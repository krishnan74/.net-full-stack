using QuizupAPI.Models;

namespace QuizupAPI.Interfaces
{
    public interface IEncryptionService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hashedPassword);
    }
}