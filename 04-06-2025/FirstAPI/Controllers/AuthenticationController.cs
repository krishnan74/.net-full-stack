using FirstAPI.Interfaces;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.AspNetCore.Mvc;
using FirstAPI.Misc;
using Microsoft.AspNetCore.Authentication;


namespace FirstAPI.Controllers
{


    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly FirstAPI.Interfaces.IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(FirstAPI.Interfaces.IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost]
        [CustomExceptionFilter]
        public async Task<ActionResult<UserLoginResponse>> UserLogin(UserLoginRequest loginRequest)
        {
            // try
            // {
            //     var result = await _authenticationService.Login(loginRequest);
            //     return Ok(result);
            // }
            // catch (Exception e)
            // {
            //     _logger.LogError(e.Message);
            //     return Unauthorized(e.Message);
            // }

            var result = await _authenticationService.Login(loginRequest);
            return Ok(result);
        }

        [HttpGet("google-signin")]
        public IActionResult GoogleSignIn(string returnUrl = "/")
        {
            var properties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties { RedirectUri = Url.Action("GoogleCallback", new { returnUrl }) };
            return Challenge(properties, "Google");
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback(string returnUrl = "/")
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Google");
            if (!authenticateResult.Succeeded || authenticateResult?.Principal == null)
            {
                return Unauthorized();
            }
            var email = authenticateResult.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var name = authenticateResult.Principal.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }
            var loginResponse = await _authenticationService.LoginWithGoogle(email, name);
            return Ok(loginResponse);
        }
    }
}