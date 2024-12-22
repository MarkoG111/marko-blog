using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer.Posts
{
    public class GetPostDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public IEnumerable<GetPostCategoriesDto> Categories { get; set; } = new List<GetPostCategoriesDto>();
    }
}