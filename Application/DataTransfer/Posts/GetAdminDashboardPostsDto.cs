using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Categories;

namespace Application.DataTransfer.Posts
{
    public class GetAdminDashboardPostsDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public IEnumerable<GetCategoriesDto> Categories { get; set; } = new List<GetCategoriesDto>();
    }
}