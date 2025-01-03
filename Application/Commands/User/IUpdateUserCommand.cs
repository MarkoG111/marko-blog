using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Users;

namespace Application.Commands.User
{
    public interface IUpdateUserCommand : ICommand<UpsertUserDto>
    {
        
    }
}