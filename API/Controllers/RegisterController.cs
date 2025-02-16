using Application;
using Application.Commands.User;
using Application.DataTransfer.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/register")]
    public class RegisterController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;

        public RegisterController(UseCaseExecutor executor)
        {
            _executor = executor;
        }

        [HttpPost]
        public IActionResult Post([FromBody] RegisterUserDto dtoRequest, [FromServices] IRegisterUserCommand command)
        {
            _executor.ExecuteCommand(command, dtoRequest);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}