using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.DataTransfer;

namespace Application.Commands.Blog
{
    public interface IUpdatePersonalBlogCommand : ICommand<UpdateBlogDto>
    {
        
    }
}