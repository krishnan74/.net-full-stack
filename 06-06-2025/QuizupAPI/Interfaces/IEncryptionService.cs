using QuizupAPI.Models;

namespace QuizupAPI.Interfaces
{
    public interface IEncryptionService
    {
        public Task<EncryptModel> EncryptData(EncryptModel data);
    }
}