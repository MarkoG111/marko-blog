using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Searches
{
    public class UserSearch : PagedSearch
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool OnlyAuthors { get; set; }
    }
}