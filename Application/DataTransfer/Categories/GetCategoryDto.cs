using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Posts;

namespace Application.DataTransfer.Categories
{
    public class GetCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<GetPostInCategoryDto> Posts { get; set; } = new List<GetPostInCategoryDto>();
    }
}