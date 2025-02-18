using Microsoft.AspNetCore.Mvc;
using API.Services;
using Application.DataTransfer.Auth;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class OAuthController : ControllerBase
    {
        private readonly OAuthService _authService;

        public OAuthController(OAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> GoogleLogin(OAuthUserDto requestDto)
        {
            try
            {
                var token = await _authService.AuthenticateUser(requestDto);
                if (token == null)
                {
                    return StatusCode(500, "Authentication failed.");
                }

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}