using System;
using System.Collections.Generic;
using System.Text;

namespace GreenShade.Blog.Domain
{
    public class UpdateArtArgs
    {

        public string Id { get; set; }
        public string BlogTitle { get; set; }
        public string PicUrl { get; set; }
        public string PicIntroduce { get; set; }
        public string BlogContent { get; set; }
    }
}
