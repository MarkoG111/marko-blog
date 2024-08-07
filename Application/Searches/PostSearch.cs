using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Searches
{
    public class PostSearch : PagedSearch
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public List<int>? CategoryIds { get; set; }
        public string SortOrder { get; set; } = "desc";
    }
}