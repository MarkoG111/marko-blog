using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IImageService
    {
        byte[] GetImage(string folderName, string imageName);
        string GetMimeType(string filePath);
    }
}