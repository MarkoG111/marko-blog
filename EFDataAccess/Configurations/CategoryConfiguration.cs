using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataAccess.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(70);

            builder.HasMany(x => x.CategoryPosts)
                   .WithOne(y => y.Category)
                   .HasForeignKey(x => x.IdCategory)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}