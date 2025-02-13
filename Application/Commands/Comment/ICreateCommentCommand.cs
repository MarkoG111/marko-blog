using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Comments;

namespace Application.Commands.Comment
{
    public interface ICreateCommentCommand : IAsyncCommand<UpsertCommentDto>
    {
        
    }
}