using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;

namespace Application.Queries.Post
{
    public interface IGetPostQuery : IQuery<GetBlogDto, int>
    {
        
    }
}