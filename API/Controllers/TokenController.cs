using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Core;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly JWTManager _manager;

        public TokenController(JWTManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginRequest request)
        {
            var token = _manager.MakeToken(request.Username, EasyEncryption.SHA.ComputeSHA256Hash(request.Password));

            if (token == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
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
