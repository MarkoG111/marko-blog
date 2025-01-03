using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.AuthorRequests;

namespace Application.Commands.AuthorRequest
{
    public interface ICreateAuthorRequestCommand : ICommand<UpsertAuthorRequestDto>
    {
        
    }
}