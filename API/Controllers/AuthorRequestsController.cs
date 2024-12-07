using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Mvc;
using Application.DataTransfer;
using Application.Queries.AuthorRequest;
using Application.Commands.AuthorRequest;
using Application.Searches;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorRequestsController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IApplicationActor _actor;

        public AuthorRequestsController(UseCaseExecutor executor, IApplicationActor actor)
        {
            _executor = executor;
            _actor = actor;
        }

        [HttpPost("/authorrequests")]
        public IActionResult Post([FromBody] AuthorRequestDto dto, [FromServices] ICreateAuthorRequestCommand command)
        {
            dto.IdUser = _actor.Id;
            _executor.ExecuteCommand(command, dto);
            return Ok(dto);
        }

        [HttpGet("/authorrequests")]
        public IActionResult Get([FromServices] IGetAuthorRequestsQuery query, [FromQuery] AuthorRequestSearch search)
        {
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpPut("/authorrequests/accept")]
        public IActionResult Accept(int id, [FromBody] UpdateAuthorRequestDto request, [FromServices] IUpdateAuthorRequestCommand command)
        {
            request.Id = id;
            request.IdRole = 2;
            _executor.ExecuteCommand(command, request);
            return Ok(request);
        }

        [HttpPut("/authorrequests/reject")]
        public IActionResult Reject(int id, [FromBody] UpdateAuthorRequestDto request, [FromServices] IUpdateAuthorRequestCommand command)
        {
            request.Id = id;
            request.IdRole = 3;
            _executor.ExecuteCommand(command, request);
            return Ok(request);
        }
    }
}