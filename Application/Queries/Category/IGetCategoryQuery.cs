using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Searches;
using Application.DataTransfer.Categories;

namespace Application.Queries.Category
{
    public interface IGetCategoryQuery : IQuery<GetCategoryDto, int>
    {
        
    }
}