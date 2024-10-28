using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Searches
{
    public class NotificationsSearch : PagedSearch
    {
        public int? IdUser { get; set; }
        public int? Type { get; set; }
    }
}