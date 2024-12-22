using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Posts;
using EFDataAccess;
using FluentValidation;

namespace Implementation.Validators.Post
{
    public class UpdatePostValidator : AbstractValidator<UpsertPostDto>
    {
        private readonly BlogContext _context;

        public UpdatePostValidator(BlogContext context)
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

            RuleForEach(x => x.CategoryIds).Must(CategoryExists)
                          .WithMessage("Category with the provided ID doesn't exist.");

            RuleFor(x => x.CategoryIds).Must(ids => ids.Distinct().Count() == ids.Count())
                .WithMessage("Duplicate categories are not allowed.");
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
