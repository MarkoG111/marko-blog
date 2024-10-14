using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Notification;
using Application.DataTransfer;
using EFDataAccess;

namespace Implementation.Commands.Notification
{
    public class EFCreateNotificationCommand : ICreateNotificationCommand
    {
        private readonly BlogContext _context;
        private readonly IApplicationActor _actor;

        public EFCreateNotificationCommand(BlogContext context, IApplicationActor actor)
        {
            _context = context;
            _actor = actor;
        }

        public int Id => (int)UseCaseEnum.EFCreateNotificationCommand;
        public string Name => UseCaseEnum.EFCreateNotificationCommand.ToString();

        public void Execute(NotificationDto request)
        {
            request.IdUser = _actor.Id;

            var notification = new Domain.Notification
            {
                IdUser = 3,
                FromIdUser = request.IdUser,
                Type = request.Type,
                Content = request.Content
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }
    }
}