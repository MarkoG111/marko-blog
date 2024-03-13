using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class BlogCategory
    {
        public int IdBlog { get; set; }
        public int IdCategory { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual Category Category { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; }

    }
}