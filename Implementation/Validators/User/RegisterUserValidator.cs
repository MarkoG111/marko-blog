using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Implementation.Validators.User
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        private readonly BlogContext _context;

        public RegisterUserValidator(BlogContext context)
        {
            _context = context;

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.").MinimumLength(2).WithMessage("Minimum length for first name is 2 characters.");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.").MinimumLength(2).WithMessage("Minimum length for last name is 2 characters.");

            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.").MinimumLength(2).WithMessage("Minimum length for username is 2 characters.").Must(DoesNotExistUsername).WithMessage("Username must be unique.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Not a valid email format.").Must(x => !context.Users.Any(user => user.Email == x)).WithMessage("Email address must be unique.");

            RuleFor(x => x.Password).NotEmpty().MinimumLength(3).WithMessage("Minimum length for password is 3 characters.");
        }

        private bool DoesNotExistUsername(string username)
        {
            var usernames = _context.Users.IgnoreQueryFilters().Select(x => x.Username);
            return !usernames.Contains(username);
        }
    }
}