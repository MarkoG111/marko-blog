using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.DataTransfer
{
    public class LikeCommentDto
    {
        public int IdUser { get; set; }
        public int IdPost { get; set; }
        public int? IdComment { get; set; }
        public LikeStatus Status { get; set; }
    }
}