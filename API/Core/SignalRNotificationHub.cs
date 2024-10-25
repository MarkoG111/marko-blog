using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Application;

namespace API.Core
{
    public class SignalRNotificationHub : INotificationHubService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationHub(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationToUser(int idUser, string message)
        {
            await _hubContext.Clients.User(idUser.ToString()).SendAsync("ReceiveNotification", message);
        }

        public async Task BroadcastMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}