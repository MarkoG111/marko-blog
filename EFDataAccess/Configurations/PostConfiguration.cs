using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataAccess.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(70);
            builder.Property(x => x.Content).IsRequired();

            builder.HasMany(x => x.PostCategories)
                .WithOne(y => y.Post)
                .HasForeignKey(x => x.IdPost)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Comments)
                .WithOne(y => y.Post)
                .HasForeignKey(x => x.IdPost);

            builder.HasMany(x => x.Likes)
                .WithOne(y => y.Post)
                .HasForeignKey(x => x.IdPost);
        }
    }
}