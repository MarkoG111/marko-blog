using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Application.DataTransfer;

namespace api.Controllers
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

        

            return Ok(new {token});
        }
    }
}