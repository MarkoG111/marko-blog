using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;

namespace Implementation.Validators.Post
{
    public class CreatePostValidator : AbstractValidator<InsertPostDto>
    {
        private readonly BlogContext _context;

        public CreatePostValidator(BlogContext context)
        {
            _context = context;

            RuleFor(x => x.Title).NotEmpty()
                .WithMessage("Title is required.");

            RuleFor(x => x.PostCategories.Count()).GreaterThan(0)
                .WithMessage("You must choose at least 1 category.");

            RuleForEach(x => x.PostCategories).ChildRules(categories =>
            {
                categories.RuleFor(x => x.IdCategory).Must(CategoryExists)
                    .WithMessage("Category with provied ID doesn't exists.");
            });

            RuleFor(x => x.PostCategories).Must(x => x.Select(y => y.IdCategory).Distinct().Count() == x.Count())
                .WithMessage("Duplicate categories not allowed.");

            RuleFor(x => x.IdImage).NotEmpty()
                .WithMessage("Image is required.");


            RuleFor(x => x.Content).NotEmpty()
                .WithMessage("Content is required.");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(x => x.Id == id);
        }
    }
}