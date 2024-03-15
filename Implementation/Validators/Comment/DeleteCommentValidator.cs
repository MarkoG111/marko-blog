using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Implementation.Validators.Comment
{
    public class DeleteCommentValidator : AbstractValidator<int>
    {
        public DeleteCommentValidator()
        {
            RuleFor(x => x).NotEmpty();
        }
    }
}