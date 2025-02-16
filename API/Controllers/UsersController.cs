using Microsoft.AspNetCore.Mvc;
using Application;
using Application.Commands.User;
using Application.DataTransfer.Users;
using Application.Queries.User;
using Application.Searches;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;

        public UsersController(UseCaseExecutor executor)
        {
            _executor = executor;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UpsertUserDto dtoRequest, [FromServices] ICreateUserCommand command)
        {
            _executor.ExecuteCommand(command, dtoRequest);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult Get([FromServices] IGetUsersQuery query, [FromQuery] UserSearch search)
        {
            return Ok(_executor.ExecuteQuery(query, search));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] IGetUserQuery query)
        {
            return Ok(_executor.ExecuteQuery(query, id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] UpsertUserDto dtoRequest, [FromServices] IUpdateUserCommand command, [FromServices] IGetUserQuery getUserQuery)
        {
            dtoRequest.Id = id;
            await _executor.ExecuteCommandAsync(command, dtoRequest);

            var updatedUser = _executor.ExecuteQuery(getUserQuery, id);

            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromServices] IDeleteUserCommand command)
        {
            _executor.ExecuteCommand(command, id);
            return NoContent();
        }

        [HttpGet("images/{image-name}")]
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