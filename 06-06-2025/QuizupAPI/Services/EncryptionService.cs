using System.Security.Cryptography;
using System.Text;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;

namespace QuizupAPI.Services
{
    public class EncryptionService : IEncryptionService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    
    }
}