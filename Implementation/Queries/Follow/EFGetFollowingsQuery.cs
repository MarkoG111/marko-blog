using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Followers;
using Application.Queries;
using Application.Queries.Follow;
using Application.Searches;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Queries.Follow
{
    public class EFGetFollowingsQuery : IGetFollowingQuery
    {
        private readonly BlogContext _context;

        public EFGetFollowingsQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetFollowingsQuery;
        public string Name => UseCaseEnum.EFGetFollowingsQuery.ToString();

        public PagedResponse<GetFollowsDto> Execute(FollowSearch search)
        {
            var following = _context.Followers.Where(f => f.IdFollower == search.IdUser).Include(f => f.FollowingUser).AsQueryable();

            var response = new PagedResponse<GetFollowsDto>
            {
                TotalCount = following.Count(),
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                
                Items = following.Skip(search.PerPage * (search.Page - 1)).Take(search.PerPage).Select(f => new GetFollowsDto
                {
                    Id = f.FollowingUser.Id,
                    FirstName = f.FollowingUser.FirstName,
                    LastName = f.FollowingUser.LastName,
                    Username = f.FollowingUser.Username,
                    Email = f.FollowingUser.Email,
                    ProfilePicture = f.FollowingUser.ProfilePicture
                }).ToList()
            };

            return response;
        }
    }
}