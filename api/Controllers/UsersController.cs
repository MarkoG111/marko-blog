using Microsoft.AspNetCore.Mvc;
using Application;
using Application.Commands.User;
using Application.DataTransfer;
using Application.Queries.User;
using Application.Searches;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;

        public UsersController(UseCaseExecutor executor)
        {
            _executor = executor;
        }

        [HttpGet]
        public IActionResult Get([FromServices] IGetUsersQuery query, [FromQuery] UserSearch search)
        {
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id, [FromServices] IGetUserQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] InsertUserDto dto, [FromServices] ICreateUserCommand command)
        {
            _executor.ExecuteCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateUserDto dto, [FromServices] IUpdateUserCommand command)
        {
            dto.Id = id;
            _executor.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteUserCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}