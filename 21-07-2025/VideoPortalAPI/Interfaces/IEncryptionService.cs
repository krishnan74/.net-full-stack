using VideoPortalAPI.Models;

namespace VideoPortalAPI.Interfaces
{
    public interface IEncryptionService
    {
        public Task<EncryptModel> EncryptData(EncryptModel data);
    }
}