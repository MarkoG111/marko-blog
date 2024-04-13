using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Queries.Comment;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Queries.Comment
{
    public class EFGetCommentQuery : IGetCommentQuery
    {
        private readonly BlogContext _context;

        public EFGetCommentQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetOneUserQuery;
        public string Name => UseCaseEnum.EFGetOneUserQuery.ToString();

        public CommentDto Execute(int search)
        {
            var comment = _context.Comments.Include(x => x.User).FirstOrDefault(y => y.Id == search);

            if (comment == null)
            {
                throw new EntityNotFoundException(search, typeof(Domain.Comment));
            }

            return new CommentDto
            {
                Id = comment.Id,
                CommentText = comment.CommentText,
                Username = comment.User.Username,
                CreatedAt = comment.CreatedAt,
                IdParent = comment.IdParent
            };
        }
    }
}