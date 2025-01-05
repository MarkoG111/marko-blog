using Microsoft.AspNetCore.Mvc;
using Application;
using Application.Commands.Category;
using Application.DataTransfer.Categories;
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
        public IActionResult Post([FromBody] UpsertCategoryDto dto, [FromServices] ICreateCategoryCommand command)
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
        public IActionResult Get(int id, [FromServices] IGetCategoryQuery query, [FromQuery] int page = 1, [FromQuery] int perPage = 3)
        {
            var search = new CategorySearch { Id = id, Page = page, PerPage = perPage };
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpPut("/categories/{id}")]
        public IActionResult Put(int id, [FromBody] UpsertCategoryDto dto, [FromServices] IUpdateCategoryCommand command)
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