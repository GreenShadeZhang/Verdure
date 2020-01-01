using GreenShade.Blog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenShade.Blog.Domain.Dto
{
    public class ArticleDto
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ArticleViews { get; set; }
        public int ArticleCommentCount { get; set; }
        public DateTime ArticleDate { get; set; }
        public int ArticleLikeCount { get; set; }
        public string UserId { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public int Status { get; set; }
        public string PicUrl { get; set; }
        public string PicInfo { get; set; }
        public ArticleDto()
        {

        }
        public ArticleDto(Article article)
        {
            if (article != null)
            {
                this.ArticleCommentCount = article.ArticleCommentCount;
                this.ArticleDate = article.ArticleDate;
                this.ArticleLikeCount = article.ArticleLikeCount;
                this.ArticleViews = article.ArticleViews;
                this.Content = article.Content;
                this.Id = article.Id;
                this.Title = article.Title;
                this.Status = article.Status;
                this.PicUrl = article.PicUrl;
                this.PicInfo = article.PicInfo;
                if (article.User != null)
                {
                    this.UserId = article.UserId;
                    this.NickName = article.User.NickName;
                    this.Avatar = article.User.Avatar;
                }
            }
        }
    }
}
