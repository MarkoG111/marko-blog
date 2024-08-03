using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Exceptions;
using Application.DataTransfer;
using Application.Commands.Like;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Like;

namespace Implementation.Commands.Like
{
    public class EFLikeCommentCommand : ILikeCommentCommand
    {
        private readonly BlogContext _context;
        private readonly LikeCommentValidator _validator;
        private readonly IApplicationActor _actor;

        public EFLikeCommentCommand(LikeCommentValidator validator, BlogContext context, IApplicationActor actor)
        {
            _validator = validator;
            _context = context;
            _actor = actor;
        }

        public int Id => (int)UseCaseEnum.EFLikeComment;
        public string Name => UseCaseEnum.EFLikeComment.ToString();

        public void Execute(LikeCommentDto request)
        {
            _validator.ValidateAndThrow(request);

            var comment = _context.Comments.FirstOrDefault(x => x.Id == request.IdComment);

            if (comment == null)
            {
                throw new EntityNotFoundException(comment.Id, typeof(Domain.Comment));
            }

            if (comment.IdUser == request.IdUser)
            {
                throw new UserLikeException(_actor);
            }

            var existingLike = _context.Likes.FirstOrDefault(x => x.IdPost == request.IdPost && x.IdUser == request.IdUser && x.IdComment == request.IdComment);

            if (existingLike != null)
            {
                // If the user already liked/disliked the comment, update the status
                existingLike.Status = request.Status;
            }
            else
            {
                var like = new Domain.Like
                {
                    IdUser = request.IdUser,
                    IdPost = request.IdPost,
                    IdComment = request.IdComment,
                    Status = request.Status
                };

                _context.Likes.Add(like);
            }

            _context.SaveChanges();
        }
    }
}