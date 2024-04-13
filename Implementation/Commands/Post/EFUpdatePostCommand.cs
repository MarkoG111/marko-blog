using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Post;
using Application.DataTransfer;
using Application.Exceptions;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Post;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Commands.Post
{
    public class EFUpdatePostCommand : IUpdatePostCommand
    {
        private readonly BlogContext _context;
        private readonly UpdatePostValidator _validator;

        public EFUpdatePostCommand(BlogContext context, UpdatePostValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public int Id => (int)UseCaseEnum.EFUpdatePostCommand;
        public string Name => UseCaseEnum.EFUpdatePostCommand.ToString();

        public void Execute(UpdateBlogDto request)
        {
            _validator.ValidateAndThrow(request);

            var post = _context.Posts.Include(x => x.PostCategories).FirstOrDefault(x => x.Id == request.Id);

            if (post == null)
            {
                throw new EntityNotFoundException(request.Id, typeof(Domain.Post));
            }

            post.Title = request.Title;
            post.Content = request.Content;
            post.IdImage = request.IdImage;
            post.ModifiedAt = DateTime.Now;

            // Ako postoje kategorije koje nisu prisutne u prosleđenom UpdateBlogDto objektu, one se označavaju kao neaktivne i obrisane, a za nove kategorije se dodaju nove veze između posta i kategorija.
            var categoryDelete = post.PostCategories.Where(x => !request.PostCategories.Contains(x.IdCategory));

            foreach (var category in categoryDelete)
            {
                category.IsActive = false;
                category.IsDeleted = true;
                category.DeletedAt = DateTime.Now;
            }

            var categoryIds = post.PostCategories.Select(x => x.IdCategory);
            var categoryInsert = request.PostCategories.Where(x => !categoryIds.Contains(x));

            foreach (var IdCategory in categoryInsert)
            {
                post.PostCategories.Add(new Domain.PostCategory
                {
                    IdCategory = IdCategory
                });
            }

            _context.SaveChanges();
        }

    }
}