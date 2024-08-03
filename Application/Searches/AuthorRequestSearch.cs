using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Searches
{
    public class AuthorRequestSearch : PagedSearch
    {
        public string? Reason { get; set; }
    }
}