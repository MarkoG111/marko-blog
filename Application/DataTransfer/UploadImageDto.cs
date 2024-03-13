using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.DataTransfer
{
    public class UploadImageDto
    {
        public IFormFile Image { get; set; }
    }
}