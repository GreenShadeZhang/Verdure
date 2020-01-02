using System;
using System.Collections.Generic;
using System.Text;

namespace GreenShade.Blog.Domain
{
    public class ChangeArtStatusArgs
    {

        public string Id { get; set; }
        public int Status { get; set; }
    }
}
