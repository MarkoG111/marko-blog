using Microsoft.AspNetCore.Mvc;
using Application;
using Application.DataTransfer.Likes;
using Application.Commands.Like;

namespace API.Controllers
{
    [ApiController]
    [Route("api/likes")]
    public class LikesController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public LikesController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost("posts/{id}")]
        public async Task<IActionResult> LikePost([FromBody] LikeDto dtoRequest, [FromServices] ILikePostCommand command)
        {
            dtoRequest.IdUser = _actor.Id;
            await _executor.ExecuteCommandAsync(command, dtoRequest);
            return Ok(dtoRequest);
        }

        [HttpPost("comments/{id}")]
        public async Task<IActionResult> LikeComment([FromBody] LikeDto dtoRequest, [FromServices] ILikeCommentCommand command)
        {
            await _executor.ExecuteCommandAsync(command, dtoRequest);
            return Ok(dtoRequest);
        }
    }
}