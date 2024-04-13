using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Application.DataTransfer;
using Application.Queries.User;
using EFDataAccess;

namespace Implementation.Queries.User
{
    public class EFGetUserQuery : IGetUserQuery
    {
        private readonly BlogContext _context;

        public EFGetUserQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetOneUserQuery;
        public string Name => UseCaseEnum.EFGetOneUserQuery.ToString();

        public SingleUserDto Execute(int search)
        {
            var user = _context.Users.Include(x => x.UserUseCases).Include(u => u.Role).FirstOrDefault(x => x.Id == search);

            return new SingleUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                RoleName = user.Role.Name,
                UserUseCases = user.UserUseCases.Select(x => new UserUseCaseDto
                {
                    IdUseCase = x.IdUseCase
                })
            };
        }
    }
}