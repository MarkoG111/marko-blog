using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.DataTransfer;

namespace Application.Queries.Comment
{
    public interface IGetCommentQuery : IQuery<CommentDto, int>
    {
        
    }
}