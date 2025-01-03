using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.AuthorRequests;
using Application.Searches;

namespace Application.Queries.AuthorRequest
{
    public interface IGetAuthorRequestsQuery : IQuery<PagedResponse<GetAuthorRequestsDto>, AuthorRequestSearch>
    {
        
    }
}