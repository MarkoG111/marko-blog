using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer
{
    public class SingleCommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int? IdParent { get; set; }
        public int IdUser { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<SingleCommentDto> Children { get; set; } = new List<SingleCommentDto>();
        public int LikesCount { get; set; }
        public List<LikeCommentDto> Likes { get; set; } = new List<LikeCommentDto>();
    }
}