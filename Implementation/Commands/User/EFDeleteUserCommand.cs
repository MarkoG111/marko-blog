using System;
using Application.Commands.User;
using Application.Exceptions;
using EFDataAccess;

namespace Implementation.Commands.User
{
    public class EFDeleteUserCommand : IDeleteUserCommand
    {
        private readonly BlogContext _context;

        public EFDeleteUserCommand(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFDeleteUserCommand;

        public string Name => UseCaseEnum.EFDeleteUserCommand.ToString();

        public void Execute(int request)
        {
            var user = _context.Users.Find(request);

            if (user == null)
            {
                throw new EntityNotFoundException(request, typeof(Domain.User));
            }

            if (user.IsDeleted == true)
            {
                throw new AlreadyDeletedException(request, typeof(Domain.User));
            }

            user.DeletedAt = DateTime.UtcNow;
            user.IsDeleted = true;
            user.IsActive = false;

            _context.SaveChanges();
        }
    }
}