using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Blog;
using Application.Exceptions;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Blog;

namespace Implementation.Commands.Blog
{
    public class EFDeleteBlogCommand : IDeleteBlogCommand
    {
        private readonly BlogContext _context;
        private readonly DeleteBlogValidator _validator;

        public EFDeleteBlogCommand(BlogContext context, DeleteBlogValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public int Id => (int)UseCaseEnum.EFDeleteBlogCommand;
        public string Name => UseCaseEnum.EFDeleteBlogCommand.ToString();

        public void Execute(int request)
        { 
            _validator.ValidateAndThrow(request);

            var blog = _context.Blogs.Find(request);

            if (blog == null)
            {
                throw new EntityNotFoundException(request, typeof(Domain.Blog));
            }

            if (blog.IsDeleted)
            {
                throw new AlreadyDeletedException(request, typeof(Domain.Blog));
            }

            blog.DeletedAt = DateTime.Now;
            blog.IsActive = false;
            blog.IsDeleted = true;

            _context.SaveChanges();
        }
    }
}