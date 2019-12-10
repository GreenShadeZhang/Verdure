using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GreenShade.Blog.Domain.Models
{
    /// <summary>
    /// 描述：此标注是对应数据库中的表名防止博客获取关联属性时匹配不到表名
    /// 时间：2019-09-22
    /// 作者：Gil Zhang
    /// </summary>
    //[Table("AspNetUsers")]
    public class ApplicationUser:IdentityUser
    {
        public string NickName { get; set; }
    }
}
