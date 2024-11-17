using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Follow;
using Application.DataTransfer;
using EFDataAccess;
using Domain;
using Application;

namespace Implementation.Commands.Follow
{
    public class EFFollowCommand : IFollowCommand
    {
        private readonly BlogContext _context;
        private readonly IApplicationActor _actor;
        private readonly INotificationHubService _notificationService;

        public EFFollowCommand(BlogContext context, IApplicationActor actor, INotificationHubService notificationService)
        {
            _context = context;
            _actor = actor;
            _notificationService = notificationService;
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

            var notification = new Domain.Notification
            {
                IdUser = request.IdFollowing,
                FromIdUser = _actor.Id,
                Type = NotificationType.Like,
                Content = $"{_actor.Identity} started following you.",
                IsRead = false
            };

            _context.Followers.Add(follow);
            _context.Notifications.Add(notification);

            _context.SaveChanges();

            _notificationService.SendNotificationToUser(request.IdFollowing, notification);
        }

    }
}