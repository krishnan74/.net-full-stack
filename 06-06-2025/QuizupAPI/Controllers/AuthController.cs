using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Interfaces;
using QuizupAPI.Models.DTOs.Authentication;
using System.Security.Claims;

namespace QuizupAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Authenticate user and return access token
        /// </summary>
        /// <param name="loginRequest">Login credentials</param>
        /// <returns>Access token and user information</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserLoginResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _authenticationService.Login(loginRequest);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during authentication. " + ex.Message });
            }
        }

        /// <summary>
        /// Refresh expired access token
        /// </summary>
        /// <param name="refreshTokenRequest">Refresh token</param>
        /// <returns>New access token</returns>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(UserLoginResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _authenticationService.RefreshTokenAsync(refreshTokenRequest);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while refreshing token. " + ex.Message });
            }
        }

        /// <summary>
        /// Get current user details
        /// </summary>
        /// <returns>Current user information</returns>
        [HttpGet("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { message = "User not authenticated." });
                }

                var userInfo = new
                {
                    Email = userEmail,
                    UserId = userId,
                    Role = userRole
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving user information. " + ex.Message });
            }
        }
    }
} 