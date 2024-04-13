using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int IdUser { get; set; }
        public int IdImage { get; set; }
        public virtual User User { get; set; }
        public virtual Image Image { get; set; }
        public virtual ICollection<PostCategory> PostCategories { get; set; } = new HashSet<PostCategory>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<Like> Likes { get; set; } = new HashSet<Like>();
    }
}