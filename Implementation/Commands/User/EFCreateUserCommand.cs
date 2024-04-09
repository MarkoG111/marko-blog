using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.User;
using Application.DataTransfer;
using Domain;
using EFDataAccess;
using FluentValidation;
using Implementation.Extensions;
using Implementation.Validators.User;

namespace Implementation.Commands.User
{
    public class EFCreateUserCommand : ICreateUserCommand
    {
        private readonly BlogContext _context;
        private readonly CreateUserValidator _validator;

        public EFCreateUserCommand(BlogContext context, CreateUserValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public int Id => (int)UseCaseEnum.EFCreateUserCommand;
        public string Name => UseCaseEnum.EFCreateUserCommand.ToString();

        public void Execute(InsertUserDto request)
        {
            _validator.ValidateAndThrow(request);

            var user = new Domain.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
            };

            int idUser = user.Id;
            foreach (var i in request.UserUseCases)
            {
                user.UserUseCases.Add(new UserUseCase
                {
                    IdUser = idUser,
                    IdUseCase = i.IdUseCase
                });
            }

            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}