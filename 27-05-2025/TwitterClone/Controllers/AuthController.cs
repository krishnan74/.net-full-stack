using TwitterClone.Interfaces;
using TwitterClone.Models;
using TwitterClone.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace TwitterClone.Controllers{
    [ApiController]
    [Route("/api/[controller]")]

    public class AuthController: ControllerBase{

        private readonly IAuthService _authService;
        public AuthController( IAuthService authService )
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest("Login data cannot be null");
            }

            var user = _authService.Login(loginModel);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public ActionResult<User> Register([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User data cannot be null");
            }

            var createdUser = _authService.Register(user);
            if (createdUser == null)
            {
                return BadRequest("Failed to register user");
            }
            return CreatedAtAction(nameof(Login), new { id = createdUser.Id }, createdUser);
        }
    }
}