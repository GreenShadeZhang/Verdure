using GreenShade.Blog.DataAccess.Data;
using GreenShade.Blog.Domain.Dto;
using GreenShade.Blog.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace GreenShade.Blog.Api.Hubs
{
    [Authorize]
    public class ChatHub:Hub
    {
        private readonly BlogSysContext _context;

        public Task SendMessage(string chatMessage)
        {
            var asb = Context.ConnectionId;
            return Clients.All.SendAsync("Send", chatMessage);

        }


        public ChatHub(BlogSysContext context)
        {
            this._context = context;
        }

        public override Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name;
         return Clients.Client(Context.ConnectionId).SendAsync("Connect", $"{name} 登录");
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var name = Context.User.Identity.Name;
            var rommId = Context.GetHttpContext().Request.Query["room_id"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, rommId);
            var chatGroup = await _context.Groups.FindAsync(rommId);
            await Clients.OthersInGroup(rommId).SendAsync("GroupSend", $"{name} 离开 {chatGroup.Title}");
           // await Clients.All.SendAsync("Send", $"{rommId} left the chat");
        
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

        public async Task SendToGroup(string groupName,int mediatype, string message)
        {
            //var chatGroup = await _context.Groups.FindAsync(groupName);
            //await Clients.Group(groupName).SendAsync("GroupRecv", $"{Context.User.Identity.Name}@{chatGroup.Title}: {message}");
            ChatMassage massage = new ChatMassage();
            massage.Content = message;
            massage.MediaType = mediatype;
            massage.RoomId = groupName;
            massage.CreateDate = DateTime.Now;
            if (Context.User.Identity.IsAuthenticated && Context.User.Claims != null)
            {
                foreach (var item in Context.User.Claims)
                {
                    if (item.Type == ClaimTypes.NameIdentifier)
                    {
                        massage.UserId = item.Value;
                    }
                }
            }
            _context.ChatMassages.Add(massage);
            int x=  await _context.SaveChangesAsync();
            var res = await _context.ChatMassages.Include(x => x.User).Where(a => a.Id == massage.Id).ToListAsync();
            if(res!=null&&res.Count>0)
            {
                massage = res.FirstOrDefault();
            }
            MsgItemDto msgItem = new MsgItemDto(massage);
            await Clients.Group(groupName).SendAsync("GroupRecv", msgItem);
        }

        public Task SendToOthersInGroup(string groupName, string name, string message)
        {
            return Clients.OthersInGroup(groupName).SendAsync("Send", $"{name}@{groupName}: {message}");
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var chatGroup = await _context.Groups.FindAsync(groupName);

            await Clients.OthersInGroup(groupName).SendAsync("GroupSend", $"{Context.User.Identity.Name} 加入 {chatGroup.Title}");
        }

        public async Task LeaveGroup(string groupName, string name)
        {
            await Clients.Group(groupName).SendAsync("GroupSend", $"{name} 离开 {groupName}");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public Task Echo(string name, string message)
        {
            return Clients.Caller.SendAsync("Send", $"{name}: {message}");
        }
    }
}
