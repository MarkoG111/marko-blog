using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataAccess.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(x => x.CommentText).IsRequired();

            builder.HasMany(x => x.ChildrenComments)
                .WithOne(x => x.ParentComment)
                .HasForeignKey(x => x.IdParent)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Likes)
                .WithOne(y => y.Comment)
                .HasForeignKey(x => x.IdComment);
        }
    }
}