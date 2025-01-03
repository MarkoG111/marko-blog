using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Notifications;
using Application.Searches;

namespace Application.Queries.Notification
{
    public interface IGetNotificationsQuery : IQuery<PagedResponse<GetNotificationDto>, NotificationsSearch>
    {
        
    }
}