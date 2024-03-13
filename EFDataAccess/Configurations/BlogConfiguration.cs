using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataAccess.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(70);
            builder.Property(x => x.Content).IsRequired();

            builder.HasMany(x => x.BlogCategories)
                .WithOne(y => y.Blog)
                .HasForeignKey(x => x.IdBlog)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Comments)
                .WithOne(y => y.Blog)
                .HasForeignKey(x => x.IdBlog);

            builder.HasMany(x => x.Likes)
                .WithOne(y => y.Blog)
                .HasForeignKey(x => x.IdBlog);
        }
    }
}