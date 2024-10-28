using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;
using Application.Queries;
using Application.Queries.Notification;
using Application.Searches;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Implementation.Queries.Notification
{
    public class EFGetNotificationsQuery : IGetNotificationsQuery
    {
        private readonly BlogContext _context;

        public EFGetNotificationsQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetNotificationsQuery;
        public string Name => UseCaseEnum.EFGetNotificationsQuery.ToString();

        public PagedResponse<GetNotificationDto> Execute(NotificationsSearch search)
        {
            var notifications = _context.Notifications.AsQueryable();

            if (search.IdUser.HasValue)
            {
                notifications = notifications.Where(x => x.IdUser == search.IdUser.Value);
            }

            if (search.Type.HasValue)
            {
                notifications = notifications.Where(x => x.Type == (NotificationType)search.Type.Value);
            }

            var skipCount = search.PerPage * (search.Page - 1);

            var response = new PagedResponse<GetNotificationDto>
            {
                CurrentPage = search.Page,
                ItemsPerPage = search.PerPage,
                TotalCount = notifications.Count(),

                Items = notifications.Skip(skipCount).Take(search.PerPage).Select(x => new GetNotificationDto
                {
                    Id = x.Id,
                    Type = x.Type,
                    Content = x.Content,
                    IsRead = x.IsRead,
                    CreatedAt = x.CreatedAt,
                }).ToList()
            };

            return response;
        }
    }
}