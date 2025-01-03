using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Comments;
using Application.Searches;

namespace Application.Queries.Comment
{
    public interface IGetCommentsQuery : IQuery<PagedResponse<GetCommentsDto>, CommentSearch>
    {

    }
}