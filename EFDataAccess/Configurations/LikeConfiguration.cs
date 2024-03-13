using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFDataAccess.Configurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.Property(x => x.IdUser).IsRequired();
            builder.Property(x => x.IdBlog).IsRequired();
            builder.Property(x => x.Status).HasColumnType("bigint");
        }
    }
}