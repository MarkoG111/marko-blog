using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer.Comments
{
    public class GetUserCommentsDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public string PostTitle { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}