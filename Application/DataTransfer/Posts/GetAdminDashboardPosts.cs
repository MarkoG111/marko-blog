using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer.Posts
{
    public class GetAdminDashboardPosts
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }
}