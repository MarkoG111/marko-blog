using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Searches
{
    public class UseCaseLogSearch : PagedSearch
    {
        public string Actor { get; set; }
        public string UseCaseName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}