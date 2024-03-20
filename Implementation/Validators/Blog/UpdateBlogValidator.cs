using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;

namespace Implementation.Validators.Blog
{
    public class UpdateBlogValidator : AbstractValidator<UpdateBlogDto>
    {
        private readonly BlogContext _context;

        public UpdateBlogValidator(BlogContext context)
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
                categories.RuleFor(x => x).Must(CategoryExists)
                    .WithMessage("Category with provied ID doesn't exists");
            });

            // x => x.BlogCategories označava svojsvto BlogCategories u UpdateBlogDto klasi. 
            // Metoda Must se koristi za definisanje pravila koje mora biti ispunjeno. U ovom slucaju prosledjujemo lambda izraz koji proverava da li su svi elementi kolekcije jedinstveni
            // x predstavlja kolekciju BlogCategories. Prvo primenjujemo Select metodu da izvucemo sve elemente kolekcije. Zatim primenjujemo Distinct metodu koja uklanja duplikate. Na kraju primenjujemo Count metodu kako bismo dobili broj jedinstvenih elemenata. Ukoliko je broj jedinstvenih elemenata jednak broju elemenata u kolekciji, to znaci da nema duplikata.
            // Ako postoje duplikati u kolekciji, bice generisana greska sa porukom.
            RuleFor(x => x.BlogCategories).Must(x => x.Select(y => y).Distinct().Count() == x.Count())
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
