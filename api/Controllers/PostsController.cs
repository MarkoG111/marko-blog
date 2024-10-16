using Microsoft.AspNetCore.Mvc;
using Application;
using Application.DataTransfer;
using Application.Commands.Post;
using Application.Queries.Post;
using Application.Searches;
using Application.Commands.Like;
using Microsoft.AspNetCore.SignalR;
using API.Core;

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
            return Ok(request);
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

        [HttpPost]
        public IActionResult Post([FromBody] InsertPostDto dto, [FromServices] ICreatePostCommand command)
        {
            _executor.ExecuteCommand(command, dto);
            return Ok(dto.Id);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdatePostDto dto, [FromServices] IUpdatePostCommand command)
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