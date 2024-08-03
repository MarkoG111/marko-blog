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
    public class CreateUserValidator : AbstractValidator<InsertUserDto>
    {
        private readonly BlogContext _context;

        public CreateUserValidator(BlogContext context)
        {
            _context = context;

            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Username).NotEmpty()
                .Must(x => !context.Users.Any(user => user.Username == x))
                .WithMessage("Username is already taken.");

            RuleFor(x => x.Password).NotEmpty().MinimumLength(3);

            RuleFor(x => x.Email).NotEmpty()
                .EmailAddress()
                .Must(x => !context.Users.Any(user => user.Email == x))
                .WithMessage("Email is already taken.");

            RuleFor(x => x.UserUseCases).Must(c => c.Select(v => v.IdUseCase).Distinct().Count() == c.Count())
                .WithMessage("Duplicate use case id's are not allowed.");

            RuleFor(x => x.UserUseCases.Count()).GreaterThan(0)
                .WithMessage("User must contain more than 0 use cases");

            RuleForEach(x => x.UserUseCases).ChildRules(n =>
            {
                n.RuleFor(x => x.IdUseCase)
                    .Must(UseCaseExists)
                    .WithMessage("{PropertyValue} use case doesn't exists.");
            });
        }

        private bool UseCaseExists(int id)
        {
            return Enum.IsDefined(typeof(UseCaseEnum), id);
        }
    }
}