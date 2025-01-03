using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.AuthorRequest;
using Application.DataTransfer.AuthorRequests;
using Application.Exceptions;
using Domain;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.AuthorRequest;

namespace Implementation.Commands.AuthorRequest
{
    public class EFCreateAuthorRequestCommand : ICreateAuthorRequestCommand
    {
        private readonly BlogContext _context;
        private readonly AuthorRequestValidator _validator;
        private readonly IApplicationActor _actor;

        public EFCreateAuthorRequestCommand(AuthorRequestValidator validator, BlogContext context, IApplicationActor actor)
        {
            _validator = validator;
            _context = context;
            _actor = actor;
        }

        public int Id => (int)UseCaseEnum.EFCreateAuthorRequestCommand;
        public string Name => UseCaseEnum.EFCreateAuthorRequestCommand.ToString();

        public void Execute(UpsertAuthorRequestDto request)
        {
            _validator.ValidateAndThrow(request);

            request.IdUser = _actor.Id;

            var existingRequest = _context.AuthorRequests.FirstOrDefault(ar => ar.IdUser == _actor.Id);

            if (existingRequest != null)
            {
                throw new AlreadyAddedException(_actor);
            }

            var authorRequest = new Domain.AuthorRequest
            {
                IdUser = request.IdUser,
                Reason = request.Reason,
                Status = request.Status
            };

            _context.AuthorRequests.Add(authorRequest);
            _context.SaveChanges();
        }
    }
}