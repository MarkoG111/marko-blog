using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Posts;
using EFDataAccess;
using FluentValidation;

namespace Implementation.Validators.Post
{
    public class CreatePostValidator : AbstractValidator<UpsertPostDto>
    {
        private readonly BlogContext _context;

        public CreatePostValidator(BlogContext context)
        {
            _context = context;

            RuleFor(x => x.Title).NotEmpty()
                .WithMessage("Title is required.");

            RuleFor(x => x.CategoryIds.Count()).GreaterThan(0)
                 .WithMessage("You must choose at least 1 category.");

            RuleFor(x => x.CategoryIds).Must(ids => ids.Distinct().Count() == ids.Count())
                .WithMessage("Duplicate categories are not allowed.");

            RuleForEach(x => x.CategoryIds).Must(CategoryExists)
                .WithMessage("Category with the provided ID doesn't exist.");

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