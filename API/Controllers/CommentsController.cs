using Application;
using Application.Commands.Comment;
using Application.DataTransfer.Comments;
using Application.Queries.Comment;
using Application.Searches;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public CommentsController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost]
        public async Task <IActionResult> Post([FromBody] UpsertCommentDto dtoRequest, [FromServices] ICreateCommentCommand command)
        {
            await _executor.ExecuteCommandAsync(command, dtoRequest);
            return Ok(dtoRequest);
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

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpsertCommentDto dtoRequest, [FromServices] IUpdatePersonalCommentCommand command)
        {
            dtoRequest.Id = id;
            dtoRequest.IdUser = _actor.Id;
            _executor.ExecuteCommand(command, dtoRequest);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteCommentCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }

        [HttpDelete("{id}/personal")]
        public IActionResult DeletePersonalComment(int id, [FromServices] IDeletePersonalCommentCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}
