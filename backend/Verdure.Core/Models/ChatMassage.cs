using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Verdure.Core
{
    public class ChatMassage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoomId { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 0为默认 -1为删除
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 0为默认文本状态
        /// </summary>
        public int MediaType { get; set; }
        /// <summary>
        /// 此导航属性 ApplicationUser类型 对应数据库里的AspNetUsers表
        /// </summary>
        public virtual ApplicationUser User { get; set; }
        public virtual ChatGroup Room { get; set; }
    }
}
