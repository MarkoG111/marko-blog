using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;

namespace Implementation.Validators.Post
{
<<<<<<< HEAD
    public class UpdatePostValidator : AbstractValidator<UpdateBlogDto>
=======
    public class UpdatePostValidator : AbstractValidator<UpdatePostDto>
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
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

            RuleForEach(x => x.PostCategories).ChildRules(categories =>
            {
                categories.RuleFor(x => x).Must(CategoryExists)
                    .WithMessage("Category with provied ID doesn't exists");
            });

            // Ako postoje duplikati u kolekciji, bice generisana greska sa porukom.
            RuleFor(x => x.PostCategories).Must(x => x.Select(y => y).Distinct().Count() == x.Count())
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
