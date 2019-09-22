﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GreenShadow.Blog.Domain.Models
{
   public class Article
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ArticleViews { get; set; }
        public int ArticleCommentCount { get; set; }
        public DateTime ArticleDate { get; set; }
        public int ArticleLikeCount { get; set; }
        /// <summary>
        /// 此导航属性 ApplicationUser类型 对应数据库里的AspNetUsers表
        /// </summary>
        public virtual ApplicationUser User { get; set; }
    }
}
