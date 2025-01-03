using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccess;
using Application.DataTransfer.UseCases;
using Application.Queries;
using Application.Searches;

namespace Implementation.Queries
{
    public class EFGetUseCaseLogsQuery : IGetUseCaseLogsQuery
    {
        private readonly BlogContext _context;

        public EFGetUseCaseLogsQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetUseCaseLogQuery;
        public string Name => UseCaseEnum.EFGetUseCaseLogQuery.ToString();

        public PagedResponse<GetUseCaseLogDto> Execute(UseCaseLogSearch search)
        {
            var query = _context.UseCaseLogs.AsQueryable();

            if (!string.IsNullOrEmpty(search.Actor) || !string.IsNullOrWhiteSpace(search.Actor))
            {
                query = query.Where(x => x.Actor.ToLower().Contains(search.Actor.ToLower()));
            }

            if (!string.IsNullOrEmpty(search.UseCaseName) || !string.IsNullOrWhiteSpace(search.UseCaseName))
            {
                query = query.Where(x => x.UseCaseName.ToLower().Contains(search.UseCaseName.ToLower()));
            }

            if (search.DateFrom != null && search.DateFrom >= search.DateTo)
            {
                query = query.Where(x => x.Date >= search.DateFrom);
            }

            if (search.DateTo != null && search.DateTo > search.DateFrom)
            {
                query = query.Where(x => x.Date <= search.DateTo);
            }

            var skipCount = search.PerPage * (search.Page - 1);

            var response = new PagedResponse<GetUseCaseLogDto>
            {
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                TotalCount = query.Count(),
                Items = query.Skip(skipCount).Take(search.PerPage).Select(x => new GetUseCaseLogDto
                {
                    Actor = x.Actor,
                    Data = x.Data,
                    Date = x.Date,
                    UseCaseName = x.UseCaseName
                }).ToList()
            };

            return response;
        }
    }
}