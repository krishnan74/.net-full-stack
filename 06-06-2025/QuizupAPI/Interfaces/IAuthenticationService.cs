using QuizupAPI.Models.DTOs.Authentication;

namespace QuizupAPI.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<UserLoginResponseDTO> Login(UserLoginRequestDTO user);
    }
}