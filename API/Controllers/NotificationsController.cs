using Microsoft.AspNetCore.Mvc;
using Application;
using Application.DataTransfer.Notifications;
using Application.Commands.Notification;
using Application.Queries.Notification;
using Application.Searches;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public NotificationsController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertNotificationDto dtoRequest, [FromServices] ICreateNotificationCommand command)
        {
            dtoRequest.FromIdUser = _actor.Id;
            await _executor.ExecuteCommandAsync(command, dtoRequest);
            return Ok();
        }

        [HttpGet]
        public IActionResult Get([FromQuery] NotificationsSearch search, [FromServices] IGetNotificationsQuery query)
        {
            search.IdUser = _actor.Id;
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpPatch("mark-as-read")]
        public IActionResult MarkAllAsRead([FromServices] IMarkAllNotificationsAsReadCommand command)
        {
            _executor.ExecuteCommand(command, _actor.Id);
            return Ok();
        }
    }
}