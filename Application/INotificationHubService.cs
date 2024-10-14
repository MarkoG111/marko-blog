using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application
{
    public interface INotificationHubService
    {
        Task SendNotificationToUser(int idUser, string message);
        Task BroadcastMessage(string message);
    }
}