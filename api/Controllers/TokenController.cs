using API.Core;
using Microsoft.AspNetCore.Mvc;
using Application.DataTransfer;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly JWTManager _manager;

        public TokenController(JWTManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginDto request)
        {
            var token = _manager.MakeToken(request.Username, request.Password);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                token
            });
        }
    }
}