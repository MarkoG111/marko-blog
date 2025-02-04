using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataAccess.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(x => x.ImagePath).IsRequired();

            builder.HasMany(x => x.Posts)
                   .WithOne(y => y.Image)
                   .HasForeignKey(x => x.IdImage)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}