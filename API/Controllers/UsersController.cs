using Microsoft.AspNetCore.Mvc;
using Application;
using Application.Commands.User;
using Application.DataTransfer.Users;
using Application.Queries.User;
using Application.Searches;
using Application.Services;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UseCaseExecutor _executor;
        private readonly IImageService _imageService;

        public UsersController(UseCaseExecutor executor, IImageService imageService)
        {
            _executor = executor;
            _imageService = imageService;
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

        [HttpGet("profile-image/{profilePicture}")]
        public IActionResult GetUserImage(string profilePicture)
        {
            if (string.IsNullOrEmpty(profilePicture))
            {
                return BadRequest("Invalid image name.");
            }

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImages", profilePicture);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("User image not found.");
            }

            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            var mimeType = _imageService.GetMimeType(profilePicture);

            return File(imageBytes, mimeType);
        }
    }
}