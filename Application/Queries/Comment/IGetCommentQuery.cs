using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Comments;

namespace Application.Queries.Comment
{
    public interface IGetCommentQuery : IQuery<GetCommentDto, int>
    {
        
    }
}