using System;
using System.Collections.Generic;
using System.Text;

namespace Verdure.ApplicationCore
{
    public class WnsSetting
    {
        /// <summary>
        /// live sdk 里的密钥
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 这个在live sdk和开发者中心都能找到
        /// </summary>
        public string Sid { get; set; }
    }
}
