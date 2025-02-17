using System.ComponentModel.DataAnnotations;
using API.Core;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly JWTManager _manager;

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public LoginController(JWTManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginRequest request)
        {
            var hashedPassword = HashPassword(request.Password);

            var token = _manager.MakeToken(request.Username, hashedPassword);

            if (token == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(new { token });
        }
    }

    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
