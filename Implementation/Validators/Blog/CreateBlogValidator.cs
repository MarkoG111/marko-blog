using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;

namespace Implementation.Validators.Blog
{
    public class CreateBlogValidator : AbstractValidator<InsertBlogDto>
    {
        private readonly BlogContext _context;

        public CreateBlogValidator(BlogContext context)
        {
            _context = context;

            RuleFor(x => x.Title).NotEmpty()
                .WithMessage("Title is required.")
                .MinimumLength(3)
                .WithMessage("Title must containt more than 3 letters.");

            RuleFor(x => x.Content).NotEmpty()
                .WithMessage("Content is required.")
                .MinimumLength(5)
                .WithMessage("Content must contain more than 5 letters.");

            RuleFor(x => x.IdImage).Must(ImageExists)
                .WithMessage("Image with provided ID doesn't exists.");

            RuleForEach(x => x.BlogCategories).ChildRules(categories =>
            {
                categories.RuleFor(x => x.IdCategory).Must(CategoryExists)
                    .WithMessage("Category with provied ID doesn't exists");
            });

            RuleFor(x => x.BlogCategories).Must(x => x.Select(y => y.IdCategory).Distinct().Count() == x.Count())
                .WithMessage("Duplicate categories not allowed.");
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(x => x.Id == id);
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(x => x.Id == id);
        }
    }
}