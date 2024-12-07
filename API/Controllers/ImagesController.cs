using Microsoft.AspNetCore.Mvc;
using Application.DataTransfer;
using Domain;
using EFDataAccess;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly BlogContext _context;

        public ImagesController(BlogContext context)
        {
            _context = context;
        }

        [HttpPost("/images")]
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

            return Ok(image);
        }

        [HttpGet("/images/{image-name}")]
        public IActionResult GetImage([FromRoute(Name = "image-name")] string imageName)
        {
            var imagePath = Path.Combine("wwwroot", "Images", imageName);
            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/jpeg");
        }
    }
}