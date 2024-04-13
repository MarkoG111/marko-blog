using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Like : BaseEntity
    {
        public int IdUser { get; set; }
        public int IdPost { get; set; }
        public int? IdComment { get; set; }
        public LikeStatus Status { get; set; }
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }
    }

    public enum LikeStatus
    {
        Liked = 1,
        Disliked = 2,
        Null = 3
    }
}