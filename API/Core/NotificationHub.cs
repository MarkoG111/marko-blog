using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace API.Core
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string idUser, string message)
        {
            await Clients.User(idUser).SendAsync("ReceiveNotification", new { content = message });
        }

        public async Task BroadcastMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task MarkAllAsRead(int IdUser)
        {
            await Clients.User(IdUser.ToString()).SendAsync("NotificationsMarkedAsRead");
        }
    }
}