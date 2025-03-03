using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Commands.User;
using Application.DataTransfer.Users;
using Application.Exceptions;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.User;
using Implementation.Extensions;
using BCrypt.Net;

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

        public async Task ExecuteAsync(UpsertUserDto request)
        {
            await _validator.ValidateAndThrowAsync(request);

            var user = await _context.Users.Include(x => x.UserUseCases).FirstOrDefaultAsync(x => x.Id == request.Id);

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
                user.ProfilePicture = await request.Image.UploadProfileImage("UserImages");
            }

            if (!string.IsNullOrEmpty(request.Password) || !string.IsNullOrWhiteSpace(request.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }

            await _context.SaveChangesAsync();
        }
    }
}