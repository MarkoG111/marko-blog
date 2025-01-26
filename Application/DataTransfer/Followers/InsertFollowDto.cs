using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer.Followers
{
    public class InsertFollowDto
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdFollowing { get; set; }
        public DateTime FollowedAt { get; set; }
    }
}