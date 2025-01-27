using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Searches
{
    public class UseCaseLogSearch : PagedSearch
    {
        public string? Actor { get; set; }
        public string? UseCaseName { get; set; }
        public DateTime? DateFrom { get; set; } = DateTime.MinValue;
        public DateTime? DateTo { get; set; } = DateTime.MaxValue;
        public string? SortOrder { get; set; } = "desc";
    }
}