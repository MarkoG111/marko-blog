using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer;

namespace Application.Services
{
    public interface INotificationService
    {
        void CreateNotification(NotificationDto dto);
    }
}