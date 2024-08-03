using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Queries;
using Application.Queries.Follow;
using Microsoft.EntityFrameworkCore;
using EFDataAccess;

namespace Implementation.Queries.Follow
{
    public class EFCheckFollowStatusQuery : ICheckFollowStatusQuery
    {
        private readonly BlogContext _context;
        private readonly IApplicationActor _actor;

        public EFCheckFollowStatusQuery(BlogContext context, IApplicationActor actor)
        {
            _context = context;
            _actor = actor;
        }

        public int Id => (int)UseCaseEnum.EFCheckFollowStatusQuery;
        public string Name => UseCaseEnum.EFCheckFollowStatusQuery.ToString();

        public bool Execute(int idFollowing)
        {
            return _context.Followers.Any(f => f.IdFollower == _actor.Id && f.IdFollowing == idFollowing);
        }
    }
}