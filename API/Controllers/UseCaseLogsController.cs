using Application;
using Application.Queries;
using Application.Searches;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UseCaseLogsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;

        public UseCaseLogsController(UseCaseExecutor executor)
        {
            _executor = executor;
        }

        [HttpGet("/usecaselogs/use-case-logs")]
        public IActionResult Get([FromQuery] UseCaseLogSearch search, [FromServices] IGetUseCaseLogsQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, search));
        }
    }
}