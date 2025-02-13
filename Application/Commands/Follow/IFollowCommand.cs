using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Followers;

namespace Application.Commands.Follow
{
    public interface IFollowCommand : IAsyncCommand<InsertFollowDto>
    {

    }
}