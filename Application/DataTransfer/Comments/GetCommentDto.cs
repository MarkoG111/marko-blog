using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer.Comments
{
    public class GetCommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostTitle { get; set; }
        public int IdPost { get; set; }
        public int IdUser { get; set; }
        public int? IdParent { get; set; }
        public bool IsDeleted { get; set; }
        public int LikesCount { get; set; }
        public List<GetCommentLikesDto> Likes { get; set; } = new List<GetCommentLikesDto>();
    }
}