using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Comments;
using Application.Exceptions;
using Application.Queries.Comment;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Queries.Comment
{
    public class EFGetOneCommentQuery : IGetCommentQuery
    {
        private readonly BlogContext _context;

        public EFGetOneCommentQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetOneCommentQuery;
        public string Name => UseCaseEnum.EFGetOneCommentQuery.ToString();

        public GetCommentDto Execute(int search)
        {
            var comment = _context.Comments.Include(x => x.User).Include(z => z.Post).Include(n => n.Likes).FirstOrDefault(y => y.Id == search);

            if (comment == null)
            {
                throw new EntityNotFoundException(search, typeof(Domain.Comment));
            }

            return new GetCommentDto
            {
                Id = comment.Id,
                CommentText = comment.CommentText,
                Username = comment.User.Username,
                FirstName = comment.User.FirstName,
                LastName = comment.User.LastName,
                PostTitle = comment.Post.Title,
                IdPost = comment.Post.Id,
                LikesCount = comment.Likes.Count(l => l.IdComment != null),
                CreatedAt = comment.CreatedAt,
                IdParent = comment.IdParent,
                IsDeleted = comment.IsDeleted
            };
        }
    }
}