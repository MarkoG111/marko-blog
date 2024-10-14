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
    public class EFMarkAllNotificationsAsReadCommand : IMarkAllNotificationsAsReadCommand
    {
        private readonly BlogContext _context;

        public EFMarkAllNotificationsAsReadCommand(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFMarkAllNotificationsAsReadCommand;
        public string Name => UseCaseEnum.EFMarkAllNotificationsAsReadCommand.ToString();

        public void Execute(int IdUser)
        {
            var unreadNotifications = _context.Notifications.Where(n => n.IdUser == IdUser && !n.IsRead).ToList();

            foreach (var unreadNotification in unreadNotifications)
            {
                unreadNotification.IsRead = true;
            }

            _context.SaveChanges();
        }
    }
}