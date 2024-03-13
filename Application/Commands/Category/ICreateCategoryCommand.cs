using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.DataTransfer;

namespace Application.Commands.Category
{
    public interface ICreateCategoryCommand : ICommand<CategoryDto>
    {
        
    }
}