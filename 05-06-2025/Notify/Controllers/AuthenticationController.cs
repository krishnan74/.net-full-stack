using Notify.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Notify.Models.DTOs;
using Notify.Misc;

namespace Notify.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly Notify.Interfaces.IAuthenticationService _authenticationService;

        public AuthenticationController(Notify.Interfaces.IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [CustomExceptionFilter]
        public async Task<ActionResult<UserLoginResponse>> UserLogin(UserLoginRequest loginRequest)
        {
            var result = await _authenticationService.Login(loginRequest);
            return Ok(result);
        }

    }
}