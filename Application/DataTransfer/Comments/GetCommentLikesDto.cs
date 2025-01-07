using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.DataTransfer.Comments
{
    public class GetCommentLikesDto
    {
        public int IdUser { get; set; }
        public int? IdComment { get; set; }
        public LikeStatus Status { get; set; }
    }
}