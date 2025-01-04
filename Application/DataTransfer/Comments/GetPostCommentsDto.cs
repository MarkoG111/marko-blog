using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Posts;

namespace Application.DataTransfer.Comments
{
    public class GetPostCommentsDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int? IdParent { get; set; }
        public int IdUser { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int LikesCount { get; set; }
        public List<GetCommentLikesDto> Likes { get; set; } = new List<GetCommentLikesDto>();
        public IEnumerable<GetPostCommentsDto> ChildrenComments { get; set; } = new List<GetPostCommentsDto>();
    }
}