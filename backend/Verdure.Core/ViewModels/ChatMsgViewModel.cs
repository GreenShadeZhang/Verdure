using System;
using System.Collections.Generic;
using System.Text;

namespace Verdure.Core.ViewModels
{
   public class ChatMsgViewModel
    {
        public string Content { get; set; }
        public int MediaType { get; set; }
        public string RoomId { get; set; }
    }
}
