using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Commands.User;
using Application.DataTransfer;
using Application.Exceptions;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.User;
using Implementation.Extensions;

namespace Implementation.Commands.User
{
    public class EFUpdateUserCommand : IUpdateUserCommand
    {
        private readonly BlogContext _context;
        private readonly UpdateUserValidator _validator;

        public EFUpdateUserCommand(BlogContext context, UpdateUserValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public int Id => (int)UseCaseEnum.EFUpdateUserCommand;
        public string Name => UseCaseEnum.EFUpdateUserCommand.ToString();

        public void Execute(UpdateUserDto request)
        {
            _validator.ValidateAndThrow(request);

            var user = _context.Users.Include(x => x.UserUseCases).FirstOrDefault(x => x.Id == request.Id);

            if (user == null)
            {
                throw new EntityNotFoundException(request.Id, typeof(Domain.User));
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Username = request.Username;
            user.Password = request.Password;

            if (request.ProfilePicture != null)
            {
                user.ProfilePicture = request.ProfilePicture.UploadImage("UserImages");
            }

            var useCaseDelete = user.UserUseCases.Where(x => !request.UserUseCases.Contains(x.IdUseCase));
            foreach (var x in useCaseDelete)
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.DeletedAt = DateTime.Now;
            }

            var userUseCaseIds = user.UserUseCases.Select(x => x.IdUseCase);
            var useCaseInsert = request.UserUseCases.Where(x => !userUseCaseIds.Contains(x));
            foreach (var id in useCaseInsert)
            {
                user.UserUseCases.Add(new Domain.UserUseCase
                {
                    IdUseCase = id
                });
            }

            _context.SaveChanges();
        }
    }
}