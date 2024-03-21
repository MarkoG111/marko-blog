using Microsoft.AspNetCore.Mvc;
using Application;
using Application.DataTransfer;
using Domain;
using EFDataAccess;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IApplicationActor _actor;
        private readonly UseCaseExecutor _executor;
        private readonly BlogContext _context;

        public ImagesController(BlogContext context, UseCaseExecutor executor, IApplicationActor actor)
        {
            _context = context;
            _executor = executor;
            _actor = actor;
        }

        [HttpPost]
        public IActionResult Post([FromForm] UploadImageDto dto)
        {
            var guid = Guid.NewGuid();
            var extension = Path.GetExtension(dto.Image.FileName);
            var newFileName = guid + extension;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            var filePath = Path.Combine(uploadsFolder, newFileName);

            Directory.CreateDirectory(uploadsFolder);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                dto.Image.CopyTo(fileStream);
            }

            var image = new Image
            {
                ImagePath = newFileName
            };

            _context.Images.Add(image);

            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}