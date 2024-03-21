using Microsoft.AspNetCore.Mvc;
using Application;
using Application.DataTransfer;
using Application.Commands.Blog;
using Application.Queries.Blog;
using Application.Searches;
using Application.Commands.Like;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public BlogsController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost]
        [Route("/api/like")]
        public IActionResult Like([FromBody] LikeDto request, [FromServices] ILikeBlogCommand command)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(request);

            request.IdUser = _actor.Id;
            _executor.ExecuteCommand(command, request);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] BlogSearch dto, [FromServices] IGetBlogsQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, dto));
        }

        [HttpGet("{id}", Name = "GetBlog")]
        public IActionResult Get(int id, [FromServices] IGetBlogQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] InsertBlogDto dto, [FromServices] ICreateBlogCommand command)
        {
            _executor.ExecuteCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateBlogDto dto, [FromServices] IUpdateBlogCommand command)
        {
            dto.Id = id;
            _executor.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteBlogCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}