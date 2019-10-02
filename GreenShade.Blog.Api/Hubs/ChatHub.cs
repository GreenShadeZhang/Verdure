using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
