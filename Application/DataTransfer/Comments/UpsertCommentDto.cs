using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer.Comments
{
    public class UpsertCommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int IdPost { get; set; }
        public int? IdParent { get; set; }
    }
}