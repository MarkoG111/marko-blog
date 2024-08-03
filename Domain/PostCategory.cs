using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class PostCategory
    {
        public int IdPost { get; set; }
        public int IdCategory { get; set; }
        public virtual Post Post { get; set; }
        public virtual Category Category { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; }
    }
}