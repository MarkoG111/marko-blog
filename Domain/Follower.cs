using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Follower
    {
        public int IdFollower { get; set; }
        public int IdFollowing { get; set; }
        public DateTime FollowedAt { get; set; }
        public DateTime? UnfollowedAt { get; set; }

        public virtual User FollowerUser { get; set; }
        public virtual User FollowingUser { get; set; }
    }
}