using Application;
using Application.DataTransfer;
using Application.Commands.Blog;
using Application.Queries.Blog;
using Application.Searches;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
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
    }
}