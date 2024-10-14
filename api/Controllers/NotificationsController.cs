using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application;
using Application.DataTransfer;
using Application.Commands.Notification;
using Application.Queries.Notification;
using Application.Searches;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public IActionResult Post([FromBody] NotificationDto dto, [FromServices] ICreateNotificationCommand command)
        {
            dto.FromIdUser = _actor.Id;
            _executor.ExecuteCommand(command, dto);
            return Ok();
        }

        [HttpPatch("mark-all-as-read")]
        public IActionResult MarkAllAsRead([FromServices] IMarkAllNotificationsAsReadCommand command)
        {
            _executor.ExecuteCommand(command, _actor.Id);
            return Ok();
        }

        [HttpGet]
        public IActionResult Get([FromQuery] NotificationsSearch search, [FromServices] IGetNotificationsQuery query)
        {
            search.IdUser = _actor.Id;
            return Ok(_executor.ExecuteQuery(query, search));
        }
    }
}