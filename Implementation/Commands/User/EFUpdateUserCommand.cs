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
            user.Email = request.Email;
            user.Password = EasyEncryption.SHA.ComputeSHA256Hash(request.Password);
            user.ProfilePicture = request.ProfilePicture.UploadImage("UserImages");

            _context.SaveChanges();
        }
    }
}