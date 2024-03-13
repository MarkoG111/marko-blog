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
        public int IdBlog { get; set; }
        public int? IdParent { get; set; }
        public virtual User User { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual Comment ParentComment { get; set; }
        public virtual ICollection<Comment> ChildrenComments { get; set; } = new HashSet<Comment>();
    }
}