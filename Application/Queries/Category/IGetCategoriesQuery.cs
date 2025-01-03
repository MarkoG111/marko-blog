using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Categories;
using Application.Searches;

namespace Application.Queries.Category
{
    public interface IGetCategoriesQuery : IQuery<PagedResponse<GetCategoriesDto>, CategorySearch>
    {
        
    }
}