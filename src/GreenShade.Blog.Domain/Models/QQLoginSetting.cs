using System;
using System.Collections.Generic;
using System.Text;

namespace GreenShade.Blog.Domain.Models
{
    public class QQLoginSetting
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string redirect_uri { get; set; }
    }
}
