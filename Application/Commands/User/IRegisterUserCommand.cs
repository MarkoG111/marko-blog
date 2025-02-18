using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Auth;

namespace Application.Commands.User
{
    public interface IRegisterUserCommand : ICommand<RegisterUserDto>
    {
        
    }
}