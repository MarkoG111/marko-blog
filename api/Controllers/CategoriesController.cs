using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Application;
using Application.Commands.Category;
using Application.DataTransfer;
using Application.Queries;
using Application.Searches;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IApplicationActor _actor;
        private readonly UseCaseExecutor _executor;

        public CategoriesController(IApplicationActor actor, UseCaseExecutor executor)
        {
            _actor = actor;
            _executor = executor;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CategoryDto dto, [FromServices] ICreateCategoryCommand command)
        {
            _executor.ExecuteCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}