using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Like;
using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Like;

namespace Implementation.Commands.Like
{
    public class EFLikeCommentCommand : ILikeCommentCommand
    {
        private readonly BlogContext _context;
        private readonly LikeCommentValidator _validator;

        public EFLikeCommentCommand(LikeCommentValidator validator, BlogContext context)
        {
            _validator = validator;
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFLikeComment;
        public string Name => UseCaseEnum.EFLikeComment.ToString();

        public void Execute(LikeCommentDto request)
        {
            _validator.ValidateAndThrow(request);

            var findLike = _context.Likes.Where(x => x.IdPost == request.IdPost && x.IdUser == request.IdUser && x.IdComment == request.IdComment).FirstOrDefault();

            if (findLike == null)
            {
                var like = new Domain.Like
                {
                    IdUser = request.IdUser,
                    IdPost = request.IdPost,
                    IdComment = request.IdComment,
                    Status = request.Status
                };

                _context.Likes.Add(like);
                _context.SaveChanges();
            }
            else
            {
                findLike.Status = request.Status;
                _context.SaveChanges();
            }
        }
    }
}