using QuizupAPI.Models.DTOs.Authentication;
using QuizupAPI.Models;

namespace QuizupAPI.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<UserLoginResponseDTO> Login(UserLoginRequestDTO user);

        public Task<UserLoginResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO refreshTokenRequest);
        public Task<bool> LogoutAsync(string username);
    }
}