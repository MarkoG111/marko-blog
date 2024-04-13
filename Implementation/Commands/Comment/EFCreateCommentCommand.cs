using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Comment;
using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Comment;

namespace Implementation.Commands.Comment
{
    public class EFCreateCommentCommand : ICreateCommentCommand
    {
        private readonly BlogContext _context;
        private readonly CreateCommentValidator _validator;
        private readonly IApplicationActor _actor;

        public EFCreateCommentCommand(BlogContext context, CreateCommentValidator validator, IApplicationActor actor)
        {
            _context = context;
            _validator = validator;
            _actor = actor;
        }

        public int Id => (int)UseCaseEnum.EFCreateCommentCommand;

        public string Name => UseCaseEnum.EFCreateCommentCommand.ToString();

        public void Execute(InsertCommentDto request)
        {
            _validator.ValidateAndThrow(request);

            request.IdUser = _actor.Id;

            var comment = new Domain.Comment
            {
                CommentText = request.CommentText,
                IdPost = request.IdPost,
                IdParent = request.IdParent,
                IdUser = request.IdUser
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
    }
}