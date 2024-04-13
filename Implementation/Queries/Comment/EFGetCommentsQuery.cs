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
            var comments = _context.Comments.Include(x => x.User).AsQueryable();

            if (!string.IsNullOrEmpty(search.Username) || !string.IsNullOrWhiteSpace(search.Username))
            {
                comments = comments.Where(c => c.User.Username.ToLower().Contains(search.Username.ToLower()));
            }

<<<<<<< HEAD
            comments = comments.OrderByDescending(c => c.CreatedAt);
=======
            var skipCount = search.PerPage * (search.Page - 1);
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532

            var response = new PagedResponse<CommentDto>
            {
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                TotalCount = comments.Count(),
<<<<<<< HEAD
                Items = comments.Select(c => new CommentDto
                {
                    Id = c.Id,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt,
                    IdParent = c.IdParent,
                    Username = c.User.Username,
                    IdUser = c.User.Id
=======
                Items = comments.Skip(skipCount).Take(search.PerPage).Select(c => new CommentDto
                {
                    Id = c.Id,
                    Comment = c.CommentText,
                    CreatedAt = c.CreatedAt,
                    Username = c.User.Username,
                    IdParent = c.IdParent
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
                }).ToList()
            };

            return response;
        }
    }
}