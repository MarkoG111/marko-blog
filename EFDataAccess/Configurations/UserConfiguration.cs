using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Username).IsRequired().HasMaxLength(40);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Username).IsUnique();

            builder.HasMany(x => x.Posts)
                .WithOne(y => y.User)
                .HasForeignKey(x => x.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Comments)
                .WithOne(y => y.User)
                .HasForeignKey(x => x.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Likes)
                .WithOne(y => y.User)
                .HasForeignKey(x => x.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Role)
                .WithMany(y => y.Users)
                .HasForeignKey(x => x.IdRole);

            builder.HasMany(x => x.UserUseCases)
                .WithOne(y => y.User)
                .HasForeignKey(x => x.IdUser)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}