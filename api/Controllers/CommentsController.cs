using Application;
using Application.Commands.Comment;
using Application.DataTransfer;
using Application.Queries.Comment;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public CommentsController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpGet("{id}", Name = "GetComment")]
        public IActionResult Get(int id, [FromQuery] CommentDto dto, [FromServices] IGetCommentQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] InsertCommentDto dto, [FromServices] ICreateCommentCommand command)
        {
            _executor.ExecuteCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] InsertCommentDto dto, [FromServices] IUpdatePersonalCommentCommand command)
        {
            dto.Id = id;
            dto.IdUser = _actor.Id;
            _executor.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteCommentCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }

        [HttpDelete("user/{id}")]
        public IActionResult DeletePersonalComment(int id, [FromServices] IDeletePersonalCommentCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}
