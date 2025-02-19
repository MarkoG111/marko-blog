using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Application.Services;

namespace Implementation.Services
{
    public class ImageService : IImageService
    {
        private readonly string _baseFolderPath = "wwwroot";

        public byte[]? GetImage(string folderName, string imageName)
        {
            var imagePath = Path.Combine(_baseFolderPath, folderName, imageName);

            if (!File.Exists(imagePath))
            {
                return null;
            }

            return File.ReadAllBytes(imagePath);
        }

        public string GetMimeType(string filePath)
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