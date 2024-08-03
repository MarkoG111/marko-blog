using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Follow;
using Application.Queries.Follow;
using Microsoft.AspNetCore.Mvc;
using Application.DataTransfer;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public IActionResult Post([FromBody] FollowDto dto, [FromServices] IFollowCommand command)
        {
            dto.IdUser = _actor.Id;
            _executor.ExecuteCommand(command, dto);
            return Ok();
        }

        [HttpDelete("unfollow/{idFollowing}")]
        public IActionResult Delete(int idFollowing, [FromServices] IUnfollowCommand command)
        {
            _executor.ExecuteCommand(command, idFollowing);
            return Ok();
        }

        [HttpGet("check/{id}")]
        public IActionResult CheckFollowStatus(int id, [FromServices] ICheckFollowStatusQuery query)
        {
            var isFollowing = _executor.ExecuteQuery(query, id);
            return Ok(new { isFollowing });
        }
    }
}