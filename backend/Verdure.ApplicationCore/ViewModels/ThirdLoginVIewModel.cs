using System;
using System.Collections.Generic;
using System.Text;

namespace Verdure.ApplicationCore.ViewModels
{
    public class ThirdLoginViewModel
    {
        /// <summary>
        /// 第三方登录code
        /// </summary>
        public string Code { get; set; }
        public int ThirdType { get; set; }

    }
    public enum LoginType
    {
        /// <summary>
        /// qq登录
        /// </summary>
        QQ = 1
    }
}
