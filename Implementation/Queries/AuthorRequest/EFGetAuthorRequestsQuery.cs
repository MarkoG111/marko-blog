using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Application.Queries;
using Application.Queries.AuthorRequest;
using Application.Searches;
using EFDataAccess;

namespace Implementation.Queries.AuthorRequest
{
    public class EFGetAuthorRequestsQuery : IGetAuthorRequestsQuery
    {
        private readonly BlogContext _context;

        public EFGetAuthorRequestsQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetAuthorRequestsQuery;
        public string Name => UseCaseEnum.EFGetAuthorRequestsQuery.ToString();

        public PagedResponse<AuthorRequestDto> Execute(AuthorRequestSearch search)
        {
            var query = _context.AuthorRequests.AsQueryable();

            if (!string.IsNullOrEmpty(search.Reason) || !string.IsNullOrWhiteSpace(search.Reason))
            {
                query = query.Where(x => x.Reason.ToLower().Contains(search.Reason.ToLower()));
            }

            query = query.Where(x => x.IsActive == true);

            var skipCount = search.PerPage * (search.Page - 1);

            var response = new PagedResponse<AuthorRequestDto>
            {
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                TotalCount = query.Count(),
                Items = query.Skip(skipCount).Take(search.PerPage).Select(x => new AuthorRequestDto
                {
                    Id = x.Id,
                    DateCreated = x.CreatedAt,
                    Reason = x.Reason,
                    IdUser = x.IdUser,
                    Username = x.User.Username,
                    ProfilePicture = x.User.ProfilePicture,
                    Status = x.Status
                }).ToList()
            };

            return response;
        }
    }
}