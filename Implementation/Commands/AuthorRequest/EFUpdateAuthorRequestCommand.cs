using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.AuthorRequest;
using Application.DataTransfer;
using Application.Exceptions;
using EFDataAccess;
using FluentValidation;
using Domain;
using Implementation.Extensions;

namespace Implementation.Commands.AuthorRequest
{
    public class EFUpdateAuthorRequestCommand : IUpdateAuthorRequestCommand
    {
        private readonly BlogContext _context;

        public EFUpdateAuthorRequestCommand(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFUpdateAuthorRequestCommand;
        public string Name => UseCaseEnum.EFUpdateAuthorRequestCommand.ToString();

        public void Execute(UpdateAuthorRequestDto request)
        {
            var authorRequest = _context.AuthorRequests.Find(request.Id);

            if (request == null)
            {
                throw new EntityNotFoundException(request.Id, typeof(Domain.AuthorRequest));
            }

            authorRequest.Status = request.Status;
            authorRequest.ModifiedAt = DateTime.Now;

            var user = _context.Users.FirstOrDefault(x => x.Id == authorRequest.IdUser);

            if (request.Status == RequestStatus.Accepted)
            {
                if (user != null)
                {
                    user.IdRole = request.IdRole;
                }
            }

            if (request.Status == RequestStatus.Rejected)
            {
                if (user != null)
                {
                    user.IdRole = request.IdRole;
                }
            }

            user.AddDefaultUseCasesForRole();

            _context.SaveChanges();
        }
    }
}