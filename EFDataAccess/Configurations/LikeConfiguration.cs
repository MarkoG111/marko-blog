using System;
using System.Collections.Generic;
using System.Text;

using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataAccess.Configurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.Property(x => x.IdUser).IsRequired();
            builder.Property(x => x.IdPost).IsRequired();
            builder.Property(x => x.IdComment).IsRequired(false);
            builder.Property(x => x.Status).HasColumnType("bigint");
        }
    }
}