using System.IdentityModel.Tokens.Jwt;
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
        public TokenService(IConfiguration configuration)
        {
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
    }
}