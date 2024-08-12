using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Application.Queries;
using Application.Queries.Comment;
using Application.Searches;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Queries.Comment
{
    public class EFGetCommentsQuery : IGetCommentsQuery
    {
        private readonly BlogContext _context;
        public EFGetCommentsQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetCommentsQuery;
        public string Name => UseCaseEnum.EFGetCommentsQuery.ToString();

        public PagedResponse<CommentDto> Execute(CommentSearch search)
        {
            var comments = _context.Comments.Include(x => x.User).Include(x => x.Likes).Include(x => x.Post).AsQueryable();

            if (!string.IsNullOrEmpty(search.Username) || !string.IsNullOrWhiteSpace(search.Username))
            {
                comments = comments.Where(c => c.User.Username.ToLower().Contains(search.Username.ToLower()));
            }

            comments = comments.OrderByDescending(c => c.CreatedAt);

            var skipCount = search.PerPage * (search.Page - 1);
            DateTime thirtyDaysAgo = DateTime.Now.AddDays(-30);

            var response = new PagedResponse<CommentDto>
            {
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                TotalCount = comments.Count(),
                LastMonthCount = comments.Where(x => x.CreatedAt >= thirtyDaysAgo).Count(),


                Items = comments.Skip(skipCount).Take(search.PerPage).Select(c => new CommentDto
                {
                    Id = c.Id,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt,
                    IdParent = c.IdParent,
                    IsDeleted = c.IsDeleted,
                    Username = c.User.Username,
                    PostTitle = c.Post.Title,
                    IdPost = c.Post.Id,
                    IdUser = c.User.Id,
                    LikesCount = c.Likes.Count(l => l.IdComment != null),
                    Likes = c.Likes.Select(l => new LikeCommentDto
                    {
                        IdComment = l.IdComment,
                        IdUser = l.IdUser,
                        Status = l.Status,
                    }).ToList()
                }).ToList()
            };

            return response;
        }
    }
}