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
using Implementation.Extensions;
using Implementation.Validators.User;

namespace Implementation.Commands.User
{
    public class EFRegisterUserCommand : IRegisterUserCommand
    {
        private readonly BlogContext _context;
        private readonly RegisterUserValidator _validator;
        private readonly IEmailSender _sender;

        public EFRegisterUserCommand(BlogContext context, IEmailSender sender, RegisterUserValidator validator)
        {
            _context = context;
            _sender = sender;
            _validator = validator;
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
                Password = request.Password,
                ProfilePicture = request.ProfilePicture != null ? request.ProfilePicture.UploadImage("UserImages") : request.ProfilePictureUrl,
                IdRole = 1 // User
            };

            user.Password = EasyEncryption.SHA.ComputeSHA256Hash(request.Password);

            user.AddDefaultUseCasesForRole();

            _context.Users.Add(user);
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