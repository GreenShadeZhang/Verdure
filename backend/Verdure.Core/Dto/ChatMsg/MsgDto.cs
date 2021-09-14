using System;
using System.Collections.Generic;
using System.Text;

namespace Verdure.Core
{
   public class MsgDto
    {
        public int PageTotal { get; set; }
        public List<MsgItemDto> Msgs { get; set; }
    }
}
