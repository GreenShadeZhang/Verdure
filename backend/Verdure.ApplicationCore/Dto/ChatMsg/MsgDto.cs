using System;
using System.Collections.Generic;
using System.Text;

namespace Verdure.ApplicationCore
{
   public class MsgDto
    {
        public int PageTotal { get; set; }
        public List<MsgItemDto> Msgs { get; set; }
    }
}
