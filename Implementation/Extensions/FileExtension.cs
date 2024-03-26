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
        public static string UploadImage(this IFormFile image, string folder)
        {
            var guid = Guid.NewGuid();

            var extension = Path.GetExtension(image.FileName);

            var newFileName = guid + extension;

            var path = Path.Combine("wwwroot", folder, newFileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return newFileName;
        }
    }
}