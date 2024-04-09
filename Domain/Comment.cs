using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Comment : BaseEntity
    {
        public string CommentText { get; set; }
        public int IdUser { get; set; }
        public int IdPost { get; set; }
        public int? IdParent { get; set; }
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment ParentComment { get; set; }
        public virtual ICollection<Comment> ChildrenComments { get; set; } = new HashSet<Comment>();
    }
}