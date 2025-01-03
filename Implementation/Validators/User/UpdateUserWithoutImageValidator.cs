using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Users;
using EFDataAccess;
using FluentValidation;
using System.IO;

namespace Implementation.Validators.User
{
    public class UpdateUserWithoutImageValidator : AbstractValidator<UpsertUserDto>
    {
        private readonly BlogContext _context;

        public UpdateUserWithoutImageValidator(BlogContext context)
        {
            _context = context;

            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();

            RuleFor(x => x.Username).NotEmpty()
                .Must((dto, name) => !context.Users.Any(g => g.Username == name && g.Id != dto.Id))
                .WithMessage("Username is already taken.");

            RuleFor(x => x.Password)
                .Matches(@"^[\w\d]{3,}$")
                .When(x => !string.IsNullOrEmpty(x.Password) || !string.IsNullOrWhiteSpace(x.Password))
                .WithMessage("Password is invalid");
        }
    }
}