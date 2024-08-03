using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Queries.Category;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Queries.Category
{
    public class EFGetCategoryQuery : IGetCategoryQuery
    {
        private readonly BlogContext _context;

        public EFGetCategoryQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetOneCategoryQuery;

        public string Name => UseCaseEnum.EFGetOneCategoryQuery.ToString();

        public CategoryDto Execute(int search)
        {
            var category = _context.Categories
                         .Include(x => x.CategoryPosts)
                         .ThenInclude(x => x.Post)
                         .ThenInclude(x => x.PostCategories)
                         .ThenInclude(x => x.Category)
                         .Include(x => x.CategoryPosts)
                         .ThenInclude(x => x.Post)
                         .ThenInclude(x => x.User)
                         .FirstOrDefault(x => x.Id == search);

            if (category == null)
            {
                throw new EntityNotFoundException(search, typeof(Domain.Category));
            }

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Posts = category.CategoryPosts.Select(x => new GetPostDto
                {
                    Id = x.Post.Id,
                    Title = x.Post.Title,
                    Content = x.Post.Content,
                    DateCreated = x.Post.CreatedAt,
                    FirstName = x.Post.User.FirstName,
                    LastName = x.Post.User.LastName,
                    ProfilePicture = x.Post.User.ProfilePicture,
                    Categories = x.Post.PostCategories.Select(y => new CategoryDto
                    {
                        Id = y.Category.Id,
                        Name = y.Category.Name
                    }).ToList()
                }).ToList()
            };
        }
    }
}