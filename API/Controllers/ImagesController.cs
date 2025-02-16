using Microsoft.AspNetCore.Mvc;
using Application.DataTransfer;
using Domain;
using EFDataAccess;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImagesController : ControllerBase
    {
        private readonly BlogContext _context;

        public ImagesController(BlogContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Post([FromForm] UploadImageDto dtoRequest)
        {
            var guid = Guid.NewGuid();
            var extension = Path.GetExtension(dtoRequest.Image.FileName);
            var newFileName = guid + extension;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            var filePath = Path.Combine(uploadsFolder, newFileName);

            Directory.CreateDirectory(uploadsFolder);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                dtoRequest.Image.CopyTo(fileStream);
            }

            var image = new Image
            {
                ImagePath = newFileName
            };

            _context.Images.Add(image);
            _context.SaveChanges();

            return Ok(image);
        }

        [HttpGet("{image-name}")]
        public IActionResult GetImage([FromRoute(Name = "image-name")] string imageName)
        {
            var imagePath = Path.Combine("wwwroot", "Images", imageName);
            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/jpeg");
        }

        [HttpPost("proxy")]
        public async Task<IActionResult> ProxyImage([FromBody] ImageProxyRequest request)
        {
            if (string.IsNullOrEmpty(request.ImageUrl))
            {
                return BadRequest("Image URL is required");
            }

            try
            {
                using var httpClient = new HttpClient();
                
                // Add headers to mimic a browser request
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                httpClient.DefaultRequestHeaders.Add("Accept", "image/*");

                var response = await httpClient.GetAsync(request.ImageUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to fetch image");
                }

                var contentType = response.Content.Headers.ContentType?.MediaType;
                if (string.IsNullOrEmpty(contentType) || !contentType.StartsWith("image/"))
                {
                    return BadRequest("Invalid image content");
                }

                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                return File(imageBytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to proxy image: {ex.Message}");
            }
        }

        [HttpGet("proxy")]
        public async Task<IActionResult> ProxyImage([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("URL is required");
            }

            try
            {
                using var httpClient = new HttpClient();

                // Add headers to mimic a browser request
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                httpClient.DefaultRequestHeaders.Add("Accept", "image/*");

                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to fetch image");
                }

                var contentType = response.Content.Headers.ContentType?.MediaType;
                if (string.IsNullOrEmpty(contentType) || !contentType.StartsWith("image/"))
                {
                    return BadRequest("Invalid image content");
                }

                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                return File(imageBytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to proxy image: {ex.Message}");
            }
        }

        public class ImageProxyRequest
        {
            [Required]
            public string ImageUrl { get; set; }
        }
    }
}