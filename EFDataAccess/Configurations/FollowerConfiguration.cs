using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataAccess.Configurations
{
    public class FollowerConfiguration : IEntityTypeConfiguration<Follower>
    {
        public void Configure(EntityTypeBuilder<Follower> builder)
        {
            builder.HasKey(x => new { x.IdFollower, x.IdFollowing });

            builder.HasOne(x => x.FollowerUser)
                .WithMany(x => x.Followers)
                .HasForeignKey(x  => x.IdFollower)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.FollowingUser)
                .WithMany(x => x.Followings)
                .HasForeignKey(x => x.IdFollowing)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}