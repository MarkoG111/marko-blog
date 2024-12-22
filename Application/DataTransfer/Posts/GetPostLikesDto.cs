using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.DataTransfer.Posts
{
    public class GetPostLikesDto
    {
        public int Id { get; set; }
        public LikeStatus Status { get; set; }
        public int IdUser { get; set; }
    }
}