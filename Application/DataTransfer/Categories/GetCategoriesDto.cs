using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Posts;

namespace Application.DataTransfer.Categories
{
    public class GetCategoriesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<GetPostDto> Posts { get; set; } = new List<GetPostDto>();
        public int TotalCount { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount => (int)Math.Ceiling((float)TotalCount / ItemsPerPage);
    }
}