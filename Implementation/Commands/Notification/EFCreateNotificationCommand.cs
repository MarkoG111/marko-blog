using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Notification;
using Application.DataTransfer.Notifications;
using Application.Services;
using EFDataAccess;
using Domain;

namespace Implementation.Commands.Notification
{
    public class EFCreateNotificationCommand : ICreateNotificationCommand
    {
        private readonly BlogContext _context;
        private readonly IApplicationActor _actor;
        private readonly INotificationService _notificationService;

        public EFCreateNotificationCommand(BlogContext context, IApplicationActor actor, INotificationService notificationService)
        {
            _context = context;
            _actor = actor;
            _notificationService = notificationService;
        }

        public int Id => (int)UseCaseEnum.EFCreateNotificationCommand;
        public string Name => UseCaseEnum.EFCreateNotificationCommand.ToString();

        public void Execute(InsertNotificationDto request)
        {
            request.IdUser = _actor.Id;

            _notificationService.CreateNotification(request);
        }
    }
}