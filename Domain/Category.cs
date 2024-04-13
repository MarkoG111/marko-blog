using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<PostCategory> CategoryPosts { get; set; } = new HashSet<PostCategory>();
    }
}