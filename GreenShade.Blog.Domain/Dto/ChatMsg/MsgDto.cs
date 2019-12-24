using System;
using System.Collections.Generic;
using System.Text;

namespace GreenShade.Blog.Domain.Dto
{
   public class MsgDto
    {
        public int PageTotal { get; set; }
        public List<MsgItemDto> Msgs { get; set; }
    }
}
