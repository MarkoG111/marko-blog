using Microsoft.AspNetCore.Mvc;
using Application;
using Application.DataTransfer.Posts;
using Application.DataTransfer.Likes;
using Application.Commands.Post;
using Application.Queries.Post;
using Application.Searches;
using Application.Commands.Like;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public PostsController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost("/posts")]
        public async Task <IActionResult> Post([FromBody] UpsertPostDto dto, [FromServices] ICreatePostCommand command)
        {
            await _executor.ExecuteCommandAsync(command, dto);
            return Ok();
        }

        [HttpGet("/posts")]
        public IActionResult Get([FromQuery] PostSearch search, [FromServices] IGetPostsQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpGet("/posts/{id}")]
        public IActionResult Get(int id, [FromServices] IGetPostQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, id));
        }

        [HttpPut("/posts/{id}")]
        public IActionResult Put(int id, [FromBody] UpsertPostDto dto, [FromServices] IUpdatePostCommand command)
        {
            dto.Id = id;
            _executor.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("/posts/{id}")]
        public IActionResult Delete(int id, [FromServices] IDeletePostCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }

        [HttpPost("/posts/{id}/like")]
        public async Task<IActionResult> Like([FromBody] LikeDto request, [FromServices] ILikePostCommand command)
        {
            request.IdUser = _actor.Id;
            await _executor.ExecuteCommandAsync(command, request);
            return Ok(request);
        }

        [HttpPut("/posts/{id}/personal")]
        public IActionResult UpdatePersonalPost(int id, [FromBody] UpsertPostDto dto, [FromServices] IUpdatePersonalPostCommand command)
        {
            dto.Id = id;
            _executor.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("/posts/{id}/personal")]
        public IActionResult DeletePersonalPost(int id, [FromServices] IDeletePersonalPostCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}