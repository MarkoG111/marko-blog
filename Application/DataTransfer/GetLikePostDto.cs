using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Domain;

namespace Application.DataTransfer
{
    public class GetLikePostDto
    {
        public int Id { get; set; }
        public LikeStatus Status { get; set; }
        public string Username { get; set; }
    }
}