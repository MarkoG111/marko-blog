using System;

using Microsoft.EntityFrameworkCore;
using Domain;
using EFDataAccess.Configurations;

namespace EFDataAccess
{
  public class BlogContext : DbContext
  {
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new BlogConfiguration());
      modelBuilder.ApplyConfiguration(new CategoryConfiguration());
      modelBuilder.ApplyConfiguration(new BlogCategoryConfiguration());
      modelBuilder.ApplyConfiguration(new ImageConfiguration());
      modelBuilder.ApplyConfiguration(new UserConfiguration());
      modelBuilder.ApplyConfiguration(new CommentConfiguration());
      modelBuilder.ApplyConfiguration(new LikeConfiguration());

      modelBuilder.Entity<Blog>().HasQueryFilter(x => !x.IsDeleted);
      modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
      modelBuilder.Entity<BlogCategory>().HasQueryFilter(x => !x.IsDeleted);
      modelBuilder.Entity<Image>().HasQueryFilter(x => !x.IsDeleted);
      modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
      modelBuilder.Entity<Role>().HasQueryFilter(x => !x.IsDeleted);
      modelBuilder.Entity<Comment>().HasQueryFilter(x => !x.IsDeleted);
      modelBuilder.Entity<Like>().HasQueryFilter(x => !x.IsDeleted);
      modelBuilder.Entity<UserUseCase>().HasQueryFilter(x => !x.IsDeleted);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-V80TVTT\SQLEXPRESS;Initial Catalog=blog;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
    }

    public override int SaveChanges()
    {
      foreach (var entry in ChangeTracker.Entries())
      {
        if (entry.Entity is BaseEntity e)
        {
          switch (entry.State)
          {
            case EntityState.Added:
              e.CreatedAt = DateTime.Now;
              e.IsActive = true;
              e.IsDeleted = false;
              e.DeletedAt = null;
              e.ModifiedAt = null;
              break;
            case EntityState.Modified:
              e.ModifiedAt = DateTime.Now;
              break;
          }
        }
      }

      return base.SaveChanges();
    }


    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BlogCategory> BlogCategories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<UserUseCase> UserUseCases { get; set; }
    public DbSet<UseCaseLog> UseCaseLogs { get; set; }
  }

}

