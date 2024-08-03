using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Application.Queries;
using Application.Queries.Category;
using Application.Searches;
using EFDataAccess;

namespace Implementation.Queries.Category
{
    public class EFGetCategoriesQuery : IGetCategoriesQuery
    {
        private readonly BlogContext _context;

        public EFGetCategoriesQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetCategoriesQuery;
        public string Name => UseCaseEnum.EFGetCategoriesQuery.ToString();

        public PagedResponse<CategoryDto> Execute(CategorySearch search)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(search.Name) || !string.IsNullOrWhiteSpace(search.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(search.Name.ToLower()));
            }

            query = query.Where(x => x.IsActive == true);

            var response = new PagedResponse<CategoryDto>
            {
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                TotalCount = query.Count(),
                Items = query.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };

            return response;
        }
    }
}