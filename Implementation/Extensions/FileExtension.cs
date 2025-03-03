using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Implementation.Extensions
{
    public static class FileExtension
    {
        public static async Task<string> UploadProfileImage(this IFormFile image, string folder, string oldFileName = null)
        {
            if (!string.IsNullOrEmpty(oldFileName))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, oldFileName);
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            var guid = Guid.NewGuid();
            var extension = Path.GetExtension(image.FileName);
            var newFileName = guid + extension;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImages");

            var path = Path.Combine(uploadsFolder, newFileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return newFileName;
        }
    }
}