
using System.Threading.Tasks;
using BankApplication.Interfaces;
using BankApplication.Models;
using BankApplication.Models.DTO;
using BankApplication.Services;
using Microsoft.AspNetCore.Mvc;


namespace BankApplication.Controllers
{


    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserAddRequestDTO userAddRequest)
        {
            if (userAddRequest == null)
            {
                return BadRequest("User data is required.");
            }

            var user = await _userService.RegisterUserAsync(userAddRequest);
            if (user == null)
            {
                return BadRequest("User registration failed.");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginRequestDTO userLoginRequest)
        {
            if (userLoginRequest == null)
            {
                return BadRequest("Login data is required.");
            }

            var isAuthenticated = await _userService.LoginUserAsync(userLoginRequest);
            if (!isAuthenticated)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok("Login successful.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser([FromBody] int userId)
        {
            if (userId == null || userId <= 0)
            {
                return BadRequest("User ID is required for logout.");
            }

            var isLoggedOut = await _userService.LogoutUserAsync(userId);
            if (!isLoggedOut)
            {
                return BadRequest("Logout failed.");
            }

            return Ok("Logout successful.");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UserUpdateRequestDTO userUpdateRequest)
        {
            if (userUpdateRequest == null || userUpdateRequest.UserId <= 0)
            {
                return BadRequest("User data is required for update.");
            }

            var updatedUser = await _userService.UpdateUserDetailsAsync(userUpdateRequest);
            if (updatedUser == null)
            {
                return BadRequest("User update failed.");
            }

            return Ok(updatedUser);
        }
    }
}