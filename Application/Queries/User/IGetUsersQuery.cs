using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Application.Searches;

namespace Application.Queries.User
{
    public interface IGetUsersQuery : IQuery<PagedResponse<UserDto>, UserSearch>
    {
        
    }
}