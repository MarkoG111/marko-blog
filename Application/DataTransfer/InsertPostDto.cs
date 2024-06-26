using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer
{
    public class InsertPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int IdImage { get; set; }
        public IEnumerable<PostCategoryDto> PostCategories { get; set; }
    }
}