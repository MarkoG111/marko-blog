using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Follow;
using Application.DataTransfer;
using EFDataAccess;

namespace Implementation.Commands.Follow
{
    public class EFFollowCommand : IFollowCommand
    {
        private readonly BlogContext _context;
        private readonly IApplicationActor _actor;

        public EFFollowCommand(BlogContext context, IApplicationActor actor)
        {
            _context = context;
            _actor = actor;
        }

        public int Id => (int)UseCaseEnum.EFFollowCommand;
        public string Name => UseCaseEnum.EFFollowCommand.ToString();

        public void Execute(FollowDto request)
        {
            request.IdUser = _actor.Id;

            var follow = new Domain.Follower
            {
                IdFollower = request.IdUser,
                IdFollowing = request.IdFollowing,
                FollowedAt = DateTime.Now
            };

            _context.Followers.Add(follow);
            _context.SaveChanges();
        }

    }
}