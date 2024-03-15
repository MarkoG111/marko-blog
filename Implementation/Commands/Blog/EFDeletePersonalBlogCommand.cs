using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Blog;
using Application.Exceptions;
using EFDataAccess;

namespace Implementation.Commands.Blog
{
    public class EFDeletePersonalBlogCommand : IDeletePersonalBlogCommand
    {
        private readonly BlogContext _context;
        private readonly IApplicationActor _actor;

        public EFDeletePersonalBlogCommand(BlogContext context, IApplicationActor actor)
        {
            _context = context;
            _actor = actor;
        }

        public int Id => (int)UseCaseEnum.EFDeletePersonalBlogCommand;
        public string Name => UseCaseEnum.EFDeletePersonalBlogCommand.ToString();

        public void Execute(int request)
        {
            var blog = _context.Blogs.Find(request);

            if (blog == null)
            {
                throw new EntityNotFoundException(request, typeof(Domain.Blog));
            }

            if (_actor.Id != blog.IdUser)
            {
                throw new UnauthorizedUserAccessException(_actor, Name);
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