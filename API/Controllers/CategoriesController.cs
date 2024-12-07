using Microsoft.AspNetCore.Mvc;
using Application;
using Application.Commands.Category;
using Application.DataTransfer;
using Application.Queries.Category;
using Application.Searches;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;

        public CategoriesController(UseCaseExecutor executor)
        {
            _executor = executor;
        }

        [HttpPost("/categories")]
        public IActionResult Post([FromBody] CategoryDto dto, [FromServices] ICreateCategoryCommand command)
        {
            _executor.ExecuteCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("/categories")]
        public IActionResult Get([FromServices] IGetCategoriesQuery query, [FromQuery] CategorySearch search)
        {
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpGet("/categories/{id}")]
        public IActionResult Get(int id, [FromServices] IGetCategoryQuery query, [FromQuery] CategorySearch search)
        {
            search.Id = id;
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpPut("/categories/{id}")]
        public IActionResult Put(int id, [FromBody] CategoryDto dto, [FromServices] IUpdateCategoryCommand command)
        {
            dto.Id = id;
            _executor.ExecuteCommand(command, dto);
            return NoContent();
        }

        [HttpDelete("/categories/{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteCategoryCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }
    }
}