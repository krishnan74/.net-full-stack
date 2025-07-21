using VideoPortalAPI.Models;

namespace VideoPortalAPI.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}