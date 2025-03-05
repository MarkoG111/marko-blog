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
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public PostsController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost]
        public async Task <IActionResult> Post([FromBody] UpsertPostDto dtoRequest, [FromServices] ICreatePostCommand command)
        {
            await _executor.ExecuteCommandAsync(command, dtoRequest);
            return Ok();
        }

        [HttpGet]
        public IActionResult Get([FromQuery] PostSearch search, [FromServices] IGetPostsQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IGetPostQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, id));
        }

        [HttpPut("{id}")]
        public async Task <IActionResult> Put(int id, [FromBody] UpsertPostDto dtoRequest, [FromServices] IUpdatePostCommand command)
        {
            dtoRequest.Id = id;
            await _executor.ExecuteCommandAsync(command, dtoRequest);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeletePostCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }

        [HttpPost("{id}/like")]
        public async Task<IActionResult> Like([FromBody] LikeDto dtoRequest, [FromServices] ILikePostCommand command)
        {
            dtoRequest.IdUser = _actor.Id;
            await _executor.ExecuteCommandAsync(command, dtoRequest);
            return Ok(dtoRequest);
        }

        [HttpPut("{id}/personal")]
        public IActionResult UpdatePersonalPost(int id, [FromBody] UpsertPostDto dtoRequest, [FromServices] IUpdatePersonalPostCommand command)
        {
            dtoRequest.Id = id;
            _executor.ExecuteCommand(command, dtoRequest);
            return NoContent();
        }

        [HttpDelete("{id}/personal")]
        public IActionResult DeletePersonalPost(int id, [FromServices] IDeletePersonalPostCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}