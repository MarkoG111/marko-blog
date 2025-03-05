using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Comment;
using Application.DataTransfer.Comments;
using Application.Exceptions;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Comment;

namespace Implementation.Commands.Comment
{
    public class EFUpdatePersonalCommentCommand : IUpdatePersonalCommentCommand
    {
        private readonly BlogContext _context;
        private readonly IApplicationActor _actor;
        private readonly UpdateCommentValidator _validator;

        public EFUpdatePersonalCommentCommand(UpdateCommentValidator validator, IApplicationActor actor, BlogContext context)
        {
            _validator = validator;
            _actor = actor;
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFUpdatePersonalCommentCommand;

        public string Name => UseCaseEnum.EFUpdatePersonalCommentCommand.ToString();

        public void Execute(UpsertCommentDto request)
        {
            _validator.ValidateAndThrow(request);

            var comment = _context.Comments.Find(request.Id);

            if (comment == null)
            {
                throw new EntityNotFoundException(request.Id, typeof(Domain.Comment));
            }

            if (_actor.Id != comment.IdUser)
            {
                throw new UnauthorizedUserAccessException(_actor, Name);
            }

            comment.CommentText = request.CommentText;
            comment.ModifiedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }
    }
}