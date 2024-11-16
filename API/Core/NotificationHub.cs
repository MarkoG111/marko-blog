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

        // Add user to group based on their idUser
        public async Task JoinGroup(string idUser)
        {
            if (string.IsNullOrEmpty(idUser))
            {
                throw new ArgumentException("ID User cannot be null or empty");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, idUser);
            Console.WriteLine($"User {idUser} joined group {Context.ConnectionId}");
        }

        // Send message to a specific group (idUser as group name)
        public async Task SendNotificationToUser(int idUser, string message)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, idUser.ToString());  // Join the group
            Console.WriteLine($"Sending notification to user {idUser}: {message}");
            await Clients.Group(idUser.ToString()).SendAsync("ReceiveNotification", message);  // Send message to user group
        }

        public async Task BroadcastMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            var idUser = Context.User?.FindFirst("IdUser")?.Value;
            Console.WriteLine($"User {idUser} connected with connection ID {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Optionally, remove user from the group when they disconnect
            var idUser = Context.User?.FindFirst("IdUser")?.Value;
            if (!string.IsNullOrEmpty(idUser))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, idUser);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task MarkAllAsRead(int IdUser)
        {
            await Clients.User(IdUser.ToString()).SendAsync("NotificationsMarkedAsRead");
        }
    }
}