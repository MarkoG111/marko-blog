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

            RuleFor(x => x.Username).NotEmpty()
                .Must((dto, name) => !context.Users.Any(g => g.Username == name && g.Id != dto.Id))
                .WithMessage("Username is already taken.");

            RuleFor(x => x.Password).NotEmpty()
                .MinimumLength(3);

            RuleForEach(x => x.UserUseCases).Must(UseCaseExist)
                .WithMessage("{PropertyValue} UseCase doesn't exist");

            RuleFor(x => x.UserUseCases).Must(c => c.Select(v => v).Distinct().Count() == c.Count())
                .WithMessage("Duplicates are not allowed.");

            RuleFor(x => x.ProfilePicture)
                .Must(img =>
                {
                    var allowedFormats = new List<string>() { ".jpg", ".jpeg", ".png", ".gif" };

                    if (img != null)
                    {
                        allowedFormats.Any(ext => ext.ToLower() == Path.GetExtension(img.FileName).ToLower());
                    }

                    return true;
                }).WithMessage("File is an invalid format");
        }

        private bool UseCaseExist(int id)
        {
            return Enum.IsDefined(typeof(UseCaseEnum), id);
        }
    }
}