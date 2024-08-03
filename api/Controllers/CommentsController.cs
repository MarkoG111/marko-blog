using Application;
using Application.Commands.Comment;
using Application.DataTransfer;
using Application.Queries.Comment;
using Application.Searches;
using Application.Commands.Like;
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

        [HttpPost("like")]
        public IActionResult Like([FromBody] LikeCommentDto request, [FromServices] ILikeCommentCommand command)
        {
            _executor.ExecuteCommand(command, request);
            return Ok(request);
        }

        [HttpGet]
        public IActionResult Get([FromServices] IGetCommentsQuery query, [FromQuery] CommentSearch search)
        {
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IGetCommentQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] InsertCommentDto dto, [FromServices] ICreateCommentCommand command)
        {
            _executor.ExecuteCommand(command, dto);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateCommentDto dto, [FromServices] IUpdatePersonalCommentCommand command)
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

        [HttpDelete("comment/{id}")]
        public IActionResult DeletePersonalComment(int id, [FromServices] IDeletePersonalCommentCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return Ok(id);
        }
    }
}
