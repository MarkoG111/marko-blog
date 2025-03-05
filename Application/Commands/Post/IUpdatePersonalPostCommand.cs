using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Posts;

namespace Application.Commands.Post
{
    public interface IUpdatePersonalPostCommand : IAsyncCommand<UpsertPostDto>
    {
        
    }
}