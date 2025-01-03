using Microsoft.AspNetCore.Mvc;
using Application;
using Application.DataTransfer.Notifications;
using Application.Commands.Notification;
using Application.Queries.Notification;
using Application.Searches;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public NotificationsController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost("/notifications")]
        public IActionResult Post([FromBody] InsertNotificationDto dto, [FromServices] ICreateNotificationCommand command)
        {
            dto.FromIdUser = _actor.Id;
            _executor.ExecuteCommand(command, dto);
            return Ok();
        }

        [HttpGet("/notifications")]
        public IActionResult Get([FromQuery] NotificationsSearch search, [FromServices] IGetNotificationsQuery query)
        {
            search.IdUser = _actor.Id;
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpPatch("/notifications/mark-as-read")]
        public IActionResult MarkAllAsRead([FromServices] IMarkAllNotificationsAsReadCommand command)
        {
            _executor.ExecuteCommand(command, _actor.Id);
            return Ok();
        }
    }
}