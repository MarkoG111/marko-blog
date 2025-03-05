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
    public class EFUnfollowCommand : IUnfollowCommand
    {
        private readonly BlogContext _context;
        private readonly IApplicationActor _actor;

        public EFUnfollowCommand(BlogContext context, IApplicationActor actor)
        {
            _context = context;
            _actor = actor;
        }

        public int Id => (int)UseCaseEnum.EFUnfollowCommand;
        public string Name => UseCaseEnum.EFUnfollowCommand.ToString();

        public void Execute(int idFollowing)
        {
            var follow = _context.Followers.FirstOrDefault(f => f.IdFollower == _actor.Id && f.IdFollowing == idFollowing);

            if (follow != null)
            {
                _context.Followers.Remove(follow);
                _context.SaveChanges();
            }
        }
    }
}