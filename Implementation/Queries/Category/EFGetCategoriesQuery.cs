using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Categories;
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

        public PagedResponse<GetCategoriesDto> Execute(CategorySearch search)
        {
            var query = _context.Categories.AsQueryable();

            var totalItems = query.Count();

            var pagedCategories = query
                .Skip((search.Page - 1) * search.PerPage)
                .Take(search.PerPage)
                .Select(x => new GetCategoriesDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            return new PagedResponse<GetCategoriesDto>
            {
                Items = pagedCategories,
                TotalCount = totalItems,
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage
            };
        }
    }
}
