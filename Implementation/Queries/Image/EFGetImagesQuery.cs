using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.DataTransfer;
using Application.Queries;
using Application.Queries.Image;
using Application.Searches;
using EFDataAccess;

namespace Implementation.Queries.Image
{
    public class EFGetImagesQuery : IGetImagesQuery
    {
        private readonly BlogContext _context;

        public EFGetImagesQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetImagesQuery;

        public string Name => UseCaseEnum.EFGetImagesQuery.ToString();

        public PagedResponse<GetImageDto> Execute(ImageSearch search)
        {
            var images = _context.Images.AsQueryable();

            if (!string.IsNullOrEmpty(search.ImagePath) || !string.IsNullOrWhiteSpace(search.ImagePath))
            {
                images = images.Where(x => x.ImagePath.ToLower().Contains(search.ImagePath.ToLower()));
            }

            var skipCount = search.PerPage * (search.Page - 1);

            var picture = new PagedResponse<GetImageDto>
            {
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                TotalCount = images.Count(),
                Items = images.Skip(skipCount).Take(search.PerPage).Select(x => new GetImageDto
                {
                    Id = x.Id,
                    ImagePath = x.ImagePath
                }).ToList()
            };

            return picture;
        }

    }
}