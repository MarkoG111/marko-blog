using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;

namespace Implementation.Validators.Category
{
    public class CreateCategoryValidator : AbstractValidator<CategoryDto>
    {
        public CreateCategoryValidator(BlogContext context)
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Category name is required.")
                .Must(name => !context.Categories.Any(c => c.Name == name))
                .WithMessage("Category name must be unique.");
        }
    }
}