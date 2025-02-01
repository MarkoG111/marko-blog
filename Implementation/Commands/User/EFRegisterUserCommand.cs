using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Email;
using Application.Commands.User;
using Application.DataTransfer.Users;
using Application.DataTransfer.Emails;
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
        private const string DefaultProfilePictureUrl = "https://i0.wp.com/florrycreativecare.com/wp-content/uploads/2020/08/blank-profile-picture-mystery-man-avatar-973460.jpg?ssl=1";

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
                Password = HashPassword(request.Password),
                ProfilePicture = DefaultProfilePictureUrl,
                IdRole = (int)RoleEnum.User 
            };

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                user.UpdateUseCasesForRole(_context);
                _context.SaveChanges();

                transaction.Commit();

                SendRegistrationEmail(user.Email);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("An error occurred while registering the user.", ex);
            }
        }

        private void SendRegistrationEmail(string email)
        {
            try
            {
                _sender.Send(new SendEmailDto
                {
                    Subject = "Registration",
                    Content = "<h2>Successfully Registered</h2>",
                    SendTo = email
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email to {email}: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            return EasyEncryption.SHA.ComputeSHA256Hash(password);
        }
    }
}