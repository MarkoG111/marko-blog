using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.DataTransfer;
using Application.Services;
using EFDataAccess;
using Domain;

namespace Implementation.Services
{
    public class NotificationService : INotificationService
    {
        private readonly BlogContext _context;
        private readonly INotificationHubService _notificationHubService;

        public NotificationService(BlogContext context, INotificationHubService notificationHubService)
        {
            _context = context;
            _notificationHubService = notificationHubService;
        }

        private string GenerateNotificationLink(NotificationDto dto)
        {
            return dto.Type switch
            {
                NotificationType.Post => $"/post/{dto.IdPost}",
                NotificationType.Comment => $"/comment/{dto.IdComment}",
                NotificationType.Like => $"/post/{dto.IdPost}",
                _ => "/notifications",
            };
        }

        public void CreateNotification(NotificationDto dto)
        {
            dto.Link = GenerateNotificationLink(dto);

            var notification = new Notification
            {
                IdUser = dto.IdUser,
                FromIdUser = dto.FromIdUser,
                Type = dto.Type,
                Content = dto.Content,
                Link = dto.Link,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();

            _notificationHubService.SendNotificationToUser(dto.IdUser, dto);
        }
    }
}