using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Queries.Category;
using Application.Searches;
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

        public CategoryDto Execute(CategorySearch search)
        {
            var category = _context.Categories
                         .Include(x => x.CategoryPosts)
                         .ThenInclude(x => x.Post)
                         .ThenInclude(x => x.PostCategories)
                         .ThenInclude(x => x.Category)
                         .Include(x => x.CategoryPosts)
                         .ThenInclude(x => x.Post)
                         .ThenInclude(x => x.User)
                         .FirstOrDefault(x => x.Id == search.Id);

            if (category == null)
            {
                throw new EntityNotFoundException(search.Id, typeof(Domain.Category));
            }

            var postsQuery = category.CategoryPosts.Select(x => x.Post).AsQueryable();

            var totalPosts = postsQuery.Count();

            var paginatedPosts = postsQuery.Skip((search.Page - 1) * search.PerPage).Take(search.PerPage).Select(post => new GetPostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                DateCreated = post.CreatedAt,
                FirstName = post.User.FirstName,
                LastName = post.User.LastName,
                ProfilePicture = post.User.ProfilePicture,
                Categories = post.PostCategories.Select(y => new CategoryDto
                {
                    Id = y.Category.Id,
                    Name = y.Category.Name
                }).ToList()
            }).ToList();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Posts = paginatedPosts,
                TotalCount = totalPosts,
                ItemsPerPage = search.PerPage,
                CurrentPage = search.Page
            };
        }
    }
}