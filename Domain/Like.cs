using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Like : BaseEntity
    {
        public int IdUser { get; set; }
        public int IdBlog { get; set; }
        public LikeStatus Status { get; set; }
        public virtual User User { get; set; }
        public virtual Blog Blog { get; set; }
    }

    public enum LikeStatus
    {
        Liked = 1,
        Disliked = 2,
        Null = 3
    }
}