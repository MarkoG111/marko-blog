using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Users;

namespace Application.Queries.User
{
    public interface IGetUserQuery : IQuery<GetUserDto, int>
    {
        
    }
}