using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Likes;

namespace Application.Commands.Like
{
    public interface IUnlikePostCommand : IAsyncCommand<LikeDto>
    {

    }
}