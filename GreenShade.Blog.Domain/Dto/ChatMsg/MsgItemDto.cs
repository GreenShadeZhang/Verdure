using GreenShade.Blog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenShade.Blog.Domain.Dto
{
    public class MsgItemDto
    {

        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserId { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public int Status { get; set; }
        public MsgItemDto()
        {

        }
        public MsgItemDto(ChatMassage massage)
        {
            if (massage != null)
            {
                this.CreateDate = massage.CreateDate;
                this.Content = massage.Content;
                this.Id = massage.Id;
                this.Status = massage.Status;
                if (massage.User != null)
                {
                    this.UserId = massage.UserId;
                    this.NickName = massage.User.NickName;
                    this.Avatar = massage.User.Avatar;
                }
            }
        }
    }
}
