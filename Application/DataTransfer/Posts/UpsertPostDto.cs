using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer.Posts
{
    public class UpsertPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int IdImage { get; set; }
        public string Content { get; set; }
        public IEnumerable<int> CategoryIds { get; set; }
    }
}