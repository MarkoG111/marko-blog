using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer
{
    public class GetPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string ImageName { get; set; }
        public int IdImage { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public IEnumerable<GetLikePostDto> Likes { get; set; } = new List<GetLikePostDto>();
        public IEnumerable<SingleCommentDto> Comments { get; set; } = new List<SingleCommentDto>();
        public IEnumerable<SingleCommentDto> ChildrenComments { get; set; } = new List<SingleCommentDto>();
    }
}