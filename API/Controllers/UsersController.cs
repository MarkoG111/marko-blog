using Microsoft.AspNetCore.Mvc;
using Application;
using Application.Commands.User;
using Application.DataTransfer.Users;
using Application.Queries.User;
using Application.Searches;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;

        public UsersController(UseCaseExecutor executor)
        {
            _executor = executor;
        }

        [HttpPost("/users")]
        public IActionResult Post([FromBody] UpsertUserDto dto, [FromServices] ICreateUserCommand command)
        {
            _executor.ExecuteCommand(command, dto);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("/users")]
        public IActionResult Get([FromServices] IGetUsersQuery query, [FromQuery] UserSearch search)
        {
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpGet("/users/{id}")]
        public IActionResult Get(int id, [FromServices] IGetUserQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, id));
        }

        [HttpPut("/users/{id}")]
        public IActionResult Put(int id, [FromForm] UpsertUserDto dto, [FromServices] IUpdateUserCommand command, [FromServices] IGetUserQuery getUserQuery)
        {
            dto.Id = id;
            _executor.ExecuteCommand(command, dto);

            var updatedUser = _executor.ExecuteQuery(getUserQuery, id);

            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpDelete("/users/{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteUserCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }

        [HttpGet("/users/images/{image-name}")]
        public IActionResult GetImage([FromRoute(Name = "image-name")] string imageName)
        {
            var imagePath = Path.Combine("wwwroot", "UserImages", imageName);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = System.IO.File.ReadAllBytes(imagePath);

            var mimeType = GetMimeType(imagePath);

            return File(imageBytes, mimeType);
        }

        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream",
            };
        }
    }
}