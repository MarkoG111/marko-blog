using Microsoft.AspNetCore.Mvc;
using Application;
using Application.DataTransfer;
using Application.Commands.Post;
using Application.Queries.Post;
using Application.Searches;
using Application.Commands.Like;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public PostsController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost("like")]
        public IActionResult Like([FromBody] LikePostDto request, [FromServices] ILikePostCommand command)
        {
            request.IdUser = _actor.Id;
            _executor.ExecuteCommand(command, request);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] PostSearch dto, [FromServices] IGetPostsQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, dto));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IGetPostQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] InsertBlogDto dto, [FromServices] ICreatePostCommand command)
        {
            _executor.ExecuteCommand(command, dto);
            return Ok(dto.Id);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateBlogDto dto, [FromServices] IUpdatePostCommand command)
        {
            dto.Id = id;
            _executor.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeletePostCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}