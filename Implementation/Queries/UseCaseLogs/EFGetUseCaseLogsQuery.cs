using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccess;
using Application.DataTransfer.UseCases;
using Application.Queries.UseCaseLogs;
using Application.Searches;

namespace Implementation.Queries.UseCaseLogs
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

            if (!string.IsNullOrWhiteSpace(search.Actor))
            {
                query = query.Where(x => x.Actor.ToLower().Contains(search.Actor.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(search.UseCaseName))
            {
                query = query.Where(x => x.UseCaseName.ToLower().Contains(search.UseCaseName.ToLower()));
            }

            if (search.DateFrom != null)
            {
                query = query.Where(x => x.Date >= search.DateFrom);
            }

            if (search.DateTo != null)
            {
                query = query.Where(x => x.Date <= search.DateTo);
            }

            if (search.SortOrder?.ToLower() == "asc")
            {
                query = query.OrderBy(x => x.Date);
            }
            else
            {
                query = query.OrderByDescending(x => x.Date);
            }

            var skipCount = search.PerPage * (search.Page - 1);

            var response = new PagedResponse<GetUseCaseLogDto>
            {
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                TotalCount = query.Count(),
                Items = query.Skip(skipCount).Take(search.PerPage).Select(x => new GetUseCaseLogDto
                {
                    Id = x.Id,
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