using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.DataTransfer
{
    public class LikeDto
    {
        public int IdUser { get; set; }
        public int IdBlog { get; set; }
        public LikeStatus Status { get; set; }
    }
}