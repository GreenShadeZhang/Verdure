using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
namespace GreenShade.Blog.Api.Hubs
{
    [Authorize]
    public class ChatHub:Hub
    {
        public Task SendMessage(string chatMessage)
        {
            var asb = Context.ConnectionId;
            return Clients.All.SendAsync("Send", chatMessage);

        }


        public override Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name;
         return Clients.All.SendAsync("Connect", $"{name} joined the chat");
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var name = Context.GetHttpContext().Request.Query["name"];
            return Clients.All.SendAsync("Send", $"{name} left the chat");
        }

        public Task Send(string name, string message)
        {
            return Clients.All.SendAsync("Send", $"{name}: {message}");
        }

        public Task SendToOthers(string name, string message)
        {
            return Clients.Others.SendAsync("Send", $"{name}: {message}");
        }

        public Task SendToConnection(string connectionId, string name, string message)
        {
            return Clients.Client(connectionId).SendAsync("Send", $"Private message from {name}: {message}");
        }

        public Task SendToGroup(string groupName, string name, string message)
        {
            return Clients.Group(groupName).SendAsync("Send", $"{name}@{groupName}: {message}");
        }

        public Task SendToOthersInGroup(string groupName, string name, string message)
        {
            return Clients.OthersInGroup(groupName).SendAsync("Send", $"{name}@{groupName}: {message}");
        }

        public async Task JoinGroup(string groupName, string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{name} joined {groupName}");
        }

        public async Task LeaveGroup(string groupName, string name)
        {
            await Clients.Group(groupName).SendAsync("Send", $"{name} left {groupName}");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public Task Echo(string name, string message)
        {
            return Clients.Caller.SendAsync("Send", $"{name}: {message}");
        }
    }
}
