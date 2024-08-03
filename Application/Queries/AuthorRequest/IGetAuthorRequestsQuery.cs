using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Application.Searches;

namespace Application.Queries.AuthorRequest
{
    public interface IGetAuthorRequestsQuery : IQuery<PagedResponse<AuthorRequestDto>, AuthorRequestSearch>
    {
        
    }
}