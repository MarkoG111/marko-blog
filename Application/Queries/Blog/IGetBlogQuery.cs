using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.DataTransfer;

namespace Application.Queries.Blog
{
    public interface IGetBlogQuery : IQuery<GetBlogDto, int>
    {
        
    }
}