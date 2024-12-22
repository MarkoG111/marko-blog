using Application;
using Application.Commands.Comment;
using Application.DataTransfer;
using Application.DataTransfer.Likes;
using Application.Queries.Comment;
using Application.Searches;
using Application.Commands.Like;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public CommentsController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost("/comments")]
        public IActionResult Post([FromBody] InsertCommentDto dto, [FromServices] ICreateCommentCommand command)
        {
            _executor.ExecuteCommand(command, dto);
            return Ok(dto);
        }

        [HttpGet("/comments")]
        public IActionResult Get([FromServices] IGetCommentsQuery query, [FromQuery] CommentSearch search)
        {
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpGet("/comments/{id}")]
        public IActionResult Get(int id, [FromServices] IGetCommentQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, id));
        }

        [HttpPut("/comments/{id}")]
        public IActionResult Put(int id, [FromBody] UpdateCommentDto dto, [FromServices] IUpdatePersonalCommentCommand command)
        {
            dto.Id = id;
            dto.IdUser = _actor.Id;
            _executor.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("/comments/{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteCommentCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }

        [HttpPost("/comments/{id}/like")]
        public IActionResult Like([FromBody] LikeCommentDto request, [FromServices] ILikeCommentCommand command)
        {
            _executor.ExecuteCommand(command, request);
            return Ok(request);
        }

        [HttpDelete("/comments/{id}/personal")]
        public IActionResult DeletePersonalComment(int id, [FromServices] IDeletePersonalCommentCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return Ok(id);
        }
    }
}
