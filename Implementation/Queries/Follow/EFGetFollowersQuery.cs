using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Application.Queries;
using Application.Queries.Follow;
using Application.Searches;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Queries.Follow
{
    public class EFGetFollowersQuery : IGetFollowersQuery
    {
        private readonly BlogContext _context;

        public EFGetFollowersQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetFollowersQuery;
        public string Name => UseCaseEnum.EFGetFollowersQuery.ToString();

        public PagedResponse<UserDto> Execute(int idUser)
        {
            var followers = _context.Followers.Where(f => f.IdFollowing == idUser).Include(f => f.FollowerUser).ToList();

            var response = new PagedResponse<UserDto>
            {
                TotalCount = followers.Count(),

                Items = followers.Select(f => new UserDto
                {
                    Id = f.FollowerUser.Id,
                    FirstName = f.FollowerUser.FirstName,
                    LastName = f.FollowerUser.LastName,
                    Username = f.FollowerUser.Username,
                    Email = f.FollowerUser.Email,
                    ProfilePicture = f.FollowerUser.ProfilePicture
                }).ToList()
            };

            return response;
        }
    }
}