using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Queries.Category;
using EFDataAccess;

namespace Implementation.Queries.Category
{
    public class EFGetCategoryQuery : IGetCategoryQuery
    {
        private readonly BlogContext _context;

        public EFGetCategoryQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetOneCategoryQuery;

        public string Name => UseCaseEnum.EFGetOneCategoryQuery.ToString();

        public CategoryDto Execute(int search)
        {
            var category = _context.Categories.Find(search);

            if (category == null)
            {
                throw new EntityNotFoundException(search, typeof(Domain.Category));
            }

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}