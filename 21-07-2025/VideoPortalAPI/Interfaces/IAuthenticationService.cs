
using VideoPortalAPI.Models.DTOs.UserLogin;

namespace VideoPortalAPI.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<UserLoginResponse> Login(UserLoginRequest user);
    }
}