using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Notifications;

namespace Application.Commands.Notification
{
    public interface ICreateNotificationCommand : ICommand<InsertNotificationDto>
    {
        
    }
}