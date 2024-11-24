using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Searches
{
    public class CategorySearch : PagedSearch
    {
        public int Id { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 3;
        public string? Name { get; set; }
    }
}