using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Comments;
using Application.DataTransfer.Likes;

namespace Application.DataTransfer.Posts
{
    public class GetPostDetailsDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public IEnumerable<GetPostCategoriesDto> Categories { get; set; } = new List<GetPostCategoriesDto>();
        public IEnumerable<GetPostLikesDto> Likes { get; set; } = new List<GetPostLikesDto>();
        public IEnumerable<GetPostCommentsDto> Comments { get; set; } = new List<GetPostCommentsDto>();
    }
}