using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;
using System.IO;

namespace Implementation.Validators.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        private readonly BlogContext _context;

        public UpdateUserValidator(BlogContext context)
        {
            _context = context;

            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();

            RuleFor(x => x.Username).NotEmpty()
                .Must((dto, name) => !context.Users.Any(g => g.Username == name && g.Id != dto.Id))
                .WithMessage("Username is already taken.");

            RuleFor(x => x.Password).NotEmpty()
                .MinimumLength(3);
        }
    }
}