using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface INotificationHubService
    {
        Task SendNotificationToUser(int idUser, object notification);
        Task BroadcastMessage(string message);
    }
}