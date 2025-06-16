using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Interfaces;
using QuizupAPI.Models.DTOs.Authentication;
using QuizupAPI.Models.DTOs.Response;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        [ProducesResponseType(typeof(ApiResponse<UserLoginResponseDTO>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
                }

                var response = await _authenticationService.Login(loginRequest);
                return Ok(ApiResponse<UserLoginResponseDTO>.SuccessResponse(response, "Login successful"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred during authentication. " + ex.Message));
            }
        }

        /// <summary>
        /// Refresh expired access token
        /// </summary>
        /// <param name="refreshTokenRequest">Refresh token</param>
        /// <returns>New access token</returns>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(ApiResponse<UserLoginResponseDTO>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
                }

                var response = await _authenticationService.RefreshTokenAsync(refreshTokenRequest);
                return Ok(ApiResponse<UserLoginResponseDTO>.SuccessResponse(response, "Token refreshed successfully"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while refreshing token. " + ex.Message));
            }
        }

        /// <summary>
        /// Get current user details
        /// </summary>
        /// <returns>Current user information</returns>
        [HttpGet("me")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("User not authenticated."));
                }

                var userInfo = new
                {
                    Username = username,
                    Role = userRole
                };

                return Ok(ApiResponse<object>.SuccessResponse(userInfo, "User information retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving user information. " + ex.Message));
            }
        }
    }
} 