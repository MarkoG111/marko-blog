using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;

namespace Implementation.Validators.Category
{
    public class UpdateCategoryValidator : AbstractValidator<CategoryDto>
    {
        public UpdateCategoryValidator(BlogContext context)
        {
            RuleFor(x => x.Name).NotEmpty()
               .WithMessage("Name cannot be empty.");

            RuleFor(x => x.Name).Must((dto, name) => !context.Categories.Any(x => x.Name == name && x.Id != dto.Id))
                .WithMessage(p => $"Category with the name of {p.Name} already exists in database.");
        }
    }
}