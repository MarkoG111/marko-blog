using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Domain;
using FluentValidation;
using EFDataAccess;

namespace Implementation.Validators.AuthorRequest
{
    public class AuthorRequestValidator : AbstractValidator<AuthorRequestDto>
    {
        public AuthorRequestValidator(BlogContext context)
        {
            RuleFor(x => x.IdUser).NotEmpty();
            RuleFor(x => x.Reason).NotEmpty();
            RuleFor(x => x.Status).NotEmpty()
                .Must(y => Enum.IsDefined(typeof(RequestStatus), y))
                .WithMessage("Status can only be 'Pending', 'Accepted' or 'Rejected'.");
        }
    }
}