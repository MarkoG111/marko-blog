using Application;
using Application.Commands.User;
using Application.DataTransfer;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;

        public RegisterController(UseCaseExecutor executor)
        {
            _executor = executor;
        }

        [HttpPost]
        public IActionResult Post([FromBody] RegisterUserDto dto, [FromServices] IRegisterUserCommand command)
        {
            _executor.ExecuteCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}