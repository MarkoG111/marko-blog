using Application;
using Application.Searches;
using Application.Commands.Follow;
using Application.Queries.Follow;
using Microsoft.AspNetCore.Mvc;
using Application.DataTransfer.Followers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/followers")]
    public class FollowersController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public FollowersController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertFollowDto dtoRequest, [FromServices] IFollowCommand command)
        {
            dtoRequest.IdUser = _actor.Id;
            await _executor.ExecuteCommandAsync(command, dtoRequest);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("{id}/followers")]
        public IActionResult GetFollowers([FromServices] IGetFollowersQuery query, [FromQuery] FollowSearch search)
        {
            var followers = _executor.ExecuteQuery(query, search);
            return Ok(followers);
        }

        [HttpGet("{id}/following")]
        public IActionResult GetFollowing([FromServices] IGetFollowingQuery query, [FromQuery] FollowSearch search)
        {
            var following = _executor.ExecuteQuery(query, search);
            return Ok(following);
        }

        [HttpDelete("{id}/unfollow")]
        public IActionResult Delete(int id, [FromServices] IUnfollowCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return Ok();
        }

        [HttpGet("{id}/check")]
        public IActionResult CheckFollowStatus(int id, [FromServices] ICheckFollowStatusQuery query)
        {
            var isFollowing = _executor.ExecuteQuery(query, id);
            return Ok(new { isFollowing });
        }
    }
}