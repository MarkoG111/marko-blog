using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Implementation.Validators.Post
{
    public class DeletePostValidator : AbstractValidator<int>
    {
        public DeletePostValidator()
        {
            RuleFor(x => x).NotEmpty();
        }
    }
}