using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Posts;
using Application.Searches;

namespace Application.Queries.Post
{
    public interface IGetPostsQuery : IQuery<PagedResponse<GetPostsDto>, PostSearch>
    {
        
    }
}