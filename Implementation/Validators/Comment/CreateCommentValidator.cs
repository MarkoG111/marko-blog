using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Comments;
using EFDataAccess;
using FluentValidation;

namespace Implementation.Validators.Comment
{
    public class CreateCommentValidator : AbstractValidator<UpsertCommentDto>
    {
        private readonly BlogContext _context;

        public CreateCommentValidator(BlogContext context)
        {
            _context = context;

            RuleFor(x => x.CommentText).NotEmpty()
                .WithMessage("Text is required.");

            RuleFor(x => x.IdPost).NotEmpty()
                .WithMessage("Post is required.")
                .Must(PostExists)
                .WithMessage(y => $"Post with ID {y.Id} doesn't exists.");

            RuleFor(x => x.IdParent).Must(IdComment => context.Comments.Any(y => y.Id == IdComment))
                .When(request => request.IdParent != null)
                .WithMessage("Parent comment doesn't exists in system.");
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(x => x.Id == id);
        }
    }
}