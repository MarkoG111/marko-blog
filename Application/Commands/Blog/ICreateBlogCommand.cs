using Application.DataTransfer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Commands.Blog
{
    public interface ICreateBlogCommand : ICommand<InsertBlogDto>
    {
        
    }
}