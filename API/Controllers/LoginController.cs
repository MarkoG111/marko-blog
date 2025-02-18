using API.Core;
using Microsoft.AspNetCore.Mvc;
using Application.DataTransfer.Auth;

namespace Api.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JWTManager _manager;

        public LoginController(JWTManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginUserDto requestDto)
        {
            var token = _manager.MakeToken(requestDto.Username, requestDto.Password);

            if (token == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(new { token });
        }
    }
}