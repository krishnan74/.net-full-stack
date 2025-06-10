using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.Extensions.Logging;
using QuizupAPI.Models.DTOs.Authentication;

namespace QuizupAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<string, User> _userRepository;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ITokenService tokenService,
                                    IEncryptionService encryptionService,
                                    IRepository<string, User> userRepository,
                                    ILogger<AuthenticationService> logger)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<UserLoginResponseDTO> Login(UserLoginRequestDTO user)
        {
            var dbUser = await _userRepository.Get(user.Username);
            if (dbUser == null)
            {
                _logger.LogCritical("User not found");
                throw new Exception("No such user");
            }
            if (user != null && user.Password != null)
            {
                var isPasswordValid = _encryptionService.VerifyPassword(user.Password, dbUser.HashedPassword);
                if (!isPasswordValid)
                {
                    _logger.LogWarning("Invalid password for user {Username}", user.Username);
                    throw new Exception("Invalid password");
                }
            }
            else
            {
                _logger.LogError("Password is required");
                throw new Exception("Password is required");
            }
            var token = _tokenService.GenerateToken(dbUser);
            return new UserLoginResponseDTO
            {
                Username = user.Username,
                Token = token,
            };
        }
    }
}