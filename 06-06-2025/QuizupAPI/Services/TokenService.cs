using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace QuizupAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _securityKey;
        private readonly IRepository<string, User> _userRepository;
        public TokenService(IConfiguration configuration, IRepository<string, User> userRepository)
        {
            _userRepository = userRepository;
            var key = configuration["Keys:JwtTokenKey"];
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("JwtTokenKey", "JWT token key is missing in configuration.");
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
        public string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.Update(user.Username, user);
            return refreshToken;
        }

        public async Task<User> ValidateRefreshTokenAsync(string username, string refreshToken)
        {
            var user = await _userRepository.Get(username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null; 
            }
            return user;
        }
    }
}
