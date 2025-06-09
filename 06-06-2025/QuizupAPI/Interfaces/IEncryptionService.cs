using QuizupAPI.Models;

namespace QuizupAPI.Interfaces
{
    public interface IEncryptionService
    {
        public EncryptModel EncryptData(EncryptModel data);
    }
}