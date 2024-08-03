using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer
{
    public class UpdateCommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int IdPost { get; set; }
        public int? IdParent { get; set; }
        public int IdUser { get; set; }
    }
}