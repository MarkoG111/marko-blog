using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Implementation.Validators.Blog
{
    public class DeleteBlogValidator : AbstractValidator<int>
    {
        public DeleteBlogValidator()
        {
            RuleFor(x => x).NotEmpty();
        }
    }
}