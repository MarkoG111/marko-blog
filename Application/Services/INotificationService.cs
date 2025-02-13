using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Notifications;

namespace Application.Services
{
    public interface INotificationService
    {
        Task CreateNotification(InsertNotificationDto dto);
    }
}