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
            var token = await _tokenService.GenerateToken(dbUser);
            var refreshToken = await _tokenService.GenerateAndSaveRefreshTokenAsync(dbUser);
            return new UserLoginResponseDTO
            {
                Username = user.Username,
                Role = dbUser.Role,
                AccessToken = token,
                RefreshToken = refreshToken,
            };
        }


        public async Task<UserLoginResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO refreshTokenRequest)
        {
            var user = await _tokenService.ValidateRefreshTokenAsync(refreshTokenRequest.Username, refreshTokenRequest.RefreshToken);
            if (user == null)
            {
                throw new Exception("Invalid refresh token");
            }
            var newAccessToken = await _tokenService.GenerateToken(user);
            var newRefreshToken = await _tokenService.GenerateAndSaveRefreshTokenAsync(user);
            
            return new UserLoginResponseDTO
            {
                Username = user.Username,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public Task<bool> LogoutAsync(string username)
        {
            try
            {

                return _tokenService.InvalidateRefreshTokenAsync(username);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during logout for user {Username}", username);
                throw new Exception("Logout failed");
            }
        }
        

    }
}