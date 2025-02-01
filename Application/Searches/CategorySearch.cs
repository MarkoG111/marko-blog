using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Searches
{
    public class CategorySearch : PagedSearch
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? GetAll { get; set; } 
    }
}