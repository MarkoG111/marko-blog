using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Categories;
using Application.DataTransfer.Posts;
using Application.Exceptions;
using Application.Queries;
using Application.Queries.Category;
using Application.Searches;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Queries.Category
{
    public class EFGetOneCategoryQuery : IGetCategoryQuery
    {
        private readonly BlogContext _context;

        public EFGetOneCategoryQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetOneCategoryQuery;
        public string Name => UseCaseEnum.EFGetOneCategoryQuery.ToString();

        public CategoryPostsResponse Execute(CategorySearch search)
        {
            search.Page = search.Page > 0 ? search.Page : 1;

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

            int pageCount = (int)Math.Ceiling((double)totalPosts / search.PerPage);

            search.Page = search.Page > pageCount ? pageCount : search.Page;

            var skipCount = search.PerPage * (search.Page - 1);

            var paginatedPosts = postsQuery.Skip(skipCount).Take(search.PerPage).Select(post => new GetPostInCategoryDto
            {
                Id = post.Id,
                Title = post.Title,
                DateCreated = post.CreatedAt,
                FirstName = post.User.FirstName,
                LastName = post.User.LastName,
                ProfilePicture = post.User.ProfilePicture,
                Categories = post.PostCategories.Select(y => new GetPostCategoriesDto
                {
                    Id = y.Category.Id,
                    Name = y.Category.Name
                }).ToList()
            }).ToList();

            return new CategoryPostsResponse
            {
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                TotalCount = totalPosts,
                Items = paginatedPosts,
                CategoryName = category.Name,
                CategoryId = category.Id
            };
        }
    }
}