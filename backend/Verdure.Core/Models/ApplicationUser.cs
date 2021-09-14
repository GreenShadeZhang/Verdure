using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Verdure.Core
{
    /// <summary>
    /// 描述：此标注是对应数据库中的表名防止博客获取关联属性时匹配不到表名
    /// 时间：2019-09-22
    /// 作者：Gil Zhang
    /// </summary>
    [Table("User")]
    public class ApplicationUser
    {
        public string NickName { get; set; }
        public string Gender { get; set; }
        public int GenderType { get; set; }
        public string City { get; set; }
        public string Year { get; set; }
        public string Avatar { get; set; }
        public string Province { get; set; }
    }
}
