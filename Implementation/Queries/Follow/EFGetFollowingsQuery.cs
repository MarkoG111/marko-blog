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
    public class EFGetFollowingsQuery : IGetFollowingQuery
    {
        private readonly BlogContext _context;

        public EFGetFollowingsQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetFollowingsQuery;
        public string Name => UseCaseEnum.EFGetFollowingsQuery.ToString();

        public PagedResponse<UserDto> Execute(int idUser)
        {
            var following = _context.Followers.Where(f => f.IdFollower == idUser).Include(f => f.FollowingUser);


            var response = new PagedResponse<UserDto>
            {
                TotalCount = following.Count(),

                Items = following.Select(f => new UserDto
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