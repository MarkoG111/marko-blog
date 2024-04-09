using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Application.DataTransfer;
using Application.Queries;
using Application.Queries.Post;
using Application.Searches;
using EFDataAccess;

namespace Implementation.Queries.Post
{
    public class EFGetPostsQuery : IGetPostsQuery
    {
        private readonly BlogContext _context;

        public EFGetPostsQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetPostsQuery;
        public string Name => UseCaseEnum.EFGetPostsQuery.ToString();

        public PagedResponse<GetPostDto> Execute(PostSearch search)
        {
            var posts = _context.Posts.Include(x => x.Comments).Include(x => x.Likes).Include(x => x.PostCategories).ThenInclude(x => x.Category).AsQueryable();

            if (!string.IsNullOrEmpty(search.Title) || !string.IsNullOrWhiteSpace(search.Title))
            {
                posts = posts.Where(x => x.Title.ToLower().Contains(search.Title.ToLower()));
            }

            if (!string.IsNullOrEmpty(search.Content) || !string.IsNullOrWhiteSpace(search.Content))
            {
                posts = posts.Where(x => x.Content.ToLower().Contains(search.Content.ToLower()));
            }

            if (search.DateFrom != null && search.DateFrom > search.DateTo)
            {
                posts = posts.Where(x => x.CreatedAt >= search.DateFrom);
            }

            if (search.DateTo != null && search.DateTo > search.DateFrom)
            {
                posts = posts.Where(x => x.CreatedAt <= search.DateTo);
            }

            if (search.IdCategory.HasValue)
            {
                posts = posts.Where(x => x.PostCategories.Any(x => x.IdCategory == search.IdCategory.Value));
            }

            var skipCount = search.PerPage * (search.Page - 1);

            var response = new PagedResponse<GetPostDto>
            {
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                TotalCount = posts.Count(),

                Items = posts.Skip(skipCount).Take(search.PerPage).Select(x => new GetPostDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    DateCreated = x.CreatedAt,
                    Username = x.User.Username,
                    IdImage = x.IdImage,
                    ImageName = x.Image.ImagePath,
                    Categories = x.PostCategories.Select(y => new CategoryDto
                    {
                        Id = y.Category.Id,
                        Name = y.Category.Name
                    }).ToList(),
                    Comments = x.Comments.Select(z => new SingleCommentDto
                    {
                        Id = z.Id,
                        Text = z.CommentText,
                        CreatedAt = z.CreatedAt,
                        Username = z.User.Username
                    }).ToList(),
                    Likes = x.Likes.Select(w => new LikePostDto
                    {
                        Id = w.Id,
                        Status = w.Status,
                        Username = w.User.Username
                    }).ToList()
                }).ToList()
            };

            return response;
        }
    }
}