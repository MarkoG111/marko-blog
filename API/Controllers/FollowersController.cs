using Application;
using Application.Searches;
using Application.Commands.Follow;
using Application.Queries.Follow;
using Microsoft.AspNetCore.Mvc;
using Application.DataTransfer.Followers;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FollowersController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public FollowersController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost("/followers")]
        public IActionResult Post([FromBody] InsertFollowDto dto, [FromServices] IFollowCommand command)
        {
            dto.IdUser = _actor.Id;
            _executor.ExecuteCommand(command, dto);
            return Ok();
        }

        [HttpGet("/followers/{id}/followers")]
        public IActionResult GetFollowers([FromServices] IGetFollowersQuery query, [FromQuery] FollowSearch search)
        {
            var followers = _executor.ExecuteQuery(query, search);
            return Ok(followers);
        }

        [HttpGet("/followers/{id}/following")]
        public IActionResult GetFollowing(int id, [FromServices] IGetFollowingQuery query)
        {
            var following = _executor.ExecuteQuery(query, id);
            return Ok(following);
        }

        [HttpDelete("/followers/{id}/unfollow")]
        public IActionResult Delete(int id, [FromServices] IUnfollowCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return Ok();
        }

        [HttpGet("/followers/{id}/check")]
        public IActionResult CheckFollowStatus(int id, [FromServices] ICheckFollowStatusQuery query)
        {
            var isFollowing = _executor.ExecuteQuery(query, id);
            return Ok(new { isFollowing });
        }
    }
}