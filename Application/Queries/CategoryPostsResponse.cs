using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Posts;

namespace Application.Queries
{
    public class CategoryPostsResponse : PagedResponse<GetPostInCategoryDto>
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}