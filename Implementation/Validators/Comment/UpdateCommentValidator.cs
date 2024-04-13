using Application.DataTransfer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Implementation.Validators.Comment
{
    public class UpdateCommentValidator : AbstractValidator<InsertCommentDto>
    {
        public UpdateCommentValidator()
        {
            RuleFor(x => x.CommentText).NotEmpty();
        }
    }
}
