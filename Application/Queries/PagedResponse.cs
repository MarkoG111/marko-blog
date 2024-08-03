using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class PagedResponse<T> where T : class
    {
        public int TotalCount { get; set; }
        public int LastMonthCount { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount => (int)Math.Ceiling((float)TotalCount / ItemsPerPage); 
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}