using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Image : BaseEntity
    {
        public string ImagePath { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();
    }
}