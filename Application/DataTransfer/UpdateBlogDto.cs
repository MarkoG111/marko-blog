using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer
{
    public class UpdateBlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int IdImage { get; set; }
        public int IdUser { get; set; }
        public ICollection<int> PostCategories { get; set; } = new List<int>();
    }
}