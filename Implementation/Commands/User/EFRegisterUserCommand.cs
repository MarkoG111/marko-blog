using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Email;
using Application.Commands.User;
using Application.DataTransfer;
using Domain;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.User;

namespace Implementation.Commands.User
{
    public class EFRegisterUserCommand : IRegisterUserCommand
    {
        private readonly BlogContext _context;
        private readonly RegisterUserValidator _validator;
        private readonly IEmailSender _sender;

        public EFRegisterUserCommand(IEmailSender sender, RegisterUserValidator validator, BlogContext context)
        {
            _sender = sender;
            _validator = validator;
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFRegisterUserCommand;
        public string Name => UseCaseEnum.EFRegisterUserCommand.ToString();

        public void Execute(RegisterUserDto request)
        {
            _validator.ValidateAndThrow(request);

            var user = new Domain.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };

            user.Password = EasyEncryption.SHA.ComputeSHA256Hash(request.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            var userForUseCases = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 };
            int id = user.Id;
            foreach (var uucId in userForUseCases)
            {
                user.UserUseCases.Add(new UserUseCase
                {
                    IdUser = id,
                    IdUseCase = uucId
                });
            }

            _context.SaveChanges();


            _sender.Send(new SendEmailDto
            {
                Subject = "Registration",
                Content = "<h2>Successfully Registered</h2>",
                SendTo = request.Email
            });
        }
    }
}