using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.DataTransfer;
using Application.Queries;
using Application.Queries.User;
using Application.Searches;
using EFDataAccess;

namespace Implementation.Queries.User
{
    public class EFGetUsersQuery : IGetUsersQuery
    {
        private readonly BlogContext _context;

        public EFGetUsersQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetUsersQuery;
        public string Name => UseCaseEnum.EFGetUsersQuery.ToString();

        public PagedResponse<UserDto> Execute(UserSearch search)
        {
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search.Username) || !string.IsNullOrWhiteSpace(search.Username))
            {
                users = users.Where(x => x.Username.ToLower().Contains(search.Username.ToLower()));
            }
            
            if (!string.IsNullOrEmpty(search.Email) || !string.IsNullOrWhiteSpace(search.Email))
            {
                users = users.Where(x => x.Email.ToLower().Contains(search.Email.ToLower()));
            }

            var skipCount = search.PerPage * (search.Page - 1);

            var response = new PagedResponse<UserDto>
            {
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                TotalCount = users.Count(),
                Items = users.Skip(skipCount).Take(search.PerPage).Select(x => new UserDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Username = x.Username,
                    Email = x.Email,
                    Password = x.Password                 
                }).ToList()
            };

            return response;
        }
    }
}
