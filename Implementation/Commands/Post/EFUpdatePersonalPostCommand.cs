using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Post;
using Application.DataTransfer;
using Application.Exceptions;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Post;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Commands.Post
{
    public class EFUpdatePersonalPostCommand : IUpdatePersonalPostCommand
    {
        private readonly BlogContext _context;
        private readonly UpdatePostValidator _validator;
        private readonly IApplicationActor _actor;

        public EFUpdatePersonalPostCommand(BlogContext context, UpdatePostValidator validator, IApplicationActor actor)
        {
            _context = context;
            _validator = validator;
            _actor = actor;
        }

        public int Id => (int)UseCaseEnum.EFUpdatePersonalPostCommand;
        public string Name => UseCaseEnum.EFUpdatePersonalPostCommand.ToString();

        public void Execute(UpdateBlogDto request)
        {
            _validator.ValidateAndThrow(request);

            var post = _context.Posts.Include(x => x.PostCategories).FirstOrDefault(x => x.Id == request.Id);

            if (post == null)
            {
                throw new EntityNotFoundException(request.Id, typeof(Domain.Post));
            }

            if (_actor.Id != post.IdUser)
            {
                throw new UnauthorizedUserAccessException(_actor, Name);
            }

            post.Title = request.Title;
            post.Content = request.Content;
            post.IdImage = request.IdImage;
            post.ModifiedAt = DateTime.Now;

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