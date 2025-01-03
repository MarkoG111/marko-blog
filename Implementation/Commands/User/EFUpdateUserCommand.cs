using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Commands.User;
using Application.DataTransfer.Users;
using Application.Exceptions;
using EFDataAccess;
using FluentValidation;
using Implementation.Extensions;
using Implementation.Validators.User;
using Implementation.Extensions;

namespace Implementation.Commands.User
{
    public class EFUpdateUserCommand : IUpdateUserCommand
    {
        private readonly BlogContext _context;
        private readonly UpdateUserValidator _validator;
        private readonly UpdateUserWithoutImageValidator _validatorWithoutImage;

        public EFUpdateUserCommand(BlogContext context, UpdateUserValidator validator, UpdateUserWithoutImageValidator validatorWithoutImage)
        {
            _context = context;
            _validator = validator;
            _validatorWithoutImage = validatorWithoutImage;
        }

        public int Id => (int)UseCaseEnum.EFUpdateUserCommand;
        public string Name => UseCaseEnum.EFUpdateUserCommand.ToString();

        public void Execute(UpsertUserDto request)
        {
            if (request.Image != null)
            {
                _validator.ValidateAndThrow(request);
            }
            else
            {
                _validatorWithoutImage.ValidateAndThrow(request);
            }

            var user = _context.Users.Include(x => x.UserUseCases).FirstOrDefault(x => x.Id == request.Id);

            if (user == null)
            {
                throw new EntityNotFoundException(request.Id, typeof(Domain.User));
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Username = request.Username;
            user.Email = request.Email;

            if (request.Image != null)
            {
                user.ProfilePicture = request.Image.UploadImage("UserImages");
            }

            if (!string.IsNullOrEmpty(request.Password) || !string.IsNullOrWhiteSpace(request.Password))
            {
                user.Password = EasyEncryption.SHA.ComputeSHA256Hash(request.Password);
            }

            _context.SaveChanges();
        }
    }
}