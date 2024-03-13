using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataAccess.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(x => x.ImagePath).IsRequired();

            builder.HasMany(x => x.Blogs)
                   .WithOne(y => y.Image)
                   .HasForeignKey(x => x.IdImage)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}