using Microsoft.AspNetCore.Mvc;
using Application;
using Application.DataTransfer.Posts;
using Application.Commands.Post;
using Application.Queries.Post;
using Application.Searches;

namespace API.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;

        public PostsController(UseCaseExecutor executor)
        {
            _executor = executor;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UpsertPostDto dtoRequest, [FromServices] ICreatePostCommand command)
        {
            await _executor.ExecuteCommandAsync(command, dtoRequest);
            return StatusCode(StatusCodes.Status201Created);
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
        public async Task<IActionResult> Put(int id, [FromBody] UpsertPostDto dtoRequest, [FromServices] IUpdatePostCommand command)
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

        [HttpPut("{id}/personal")]
        public async Task<IActionResult> UpdatePersonalPost(int id, [FromBody] UpsertPostDto dtoRequest, [FromServices] IUpdatePersonalPostCommand command)
        {
            dtoRequest.Id = id;
            await _executor.ExecuteCommandAsync(command, dtoRequest);
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