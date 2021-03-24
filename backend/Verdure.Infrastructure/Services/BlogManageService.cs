using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Verdure.ApplicationCore;

namespace Verdure.Infrastructure
{
    public class BlogManageService : IBlogManageService
    {
        private readonly IBlogRepository _blogRepository;

        private readonly IHttpContextAccessor _contextAccessor;
        public BlogManageService(IBlogRepository blogRepository, IHttpContextAccessor contextAccessor)
        {
            _blogRepository = blogRepository;
            _contextAccessor = contextAccessor;
        }
        public async Task<Article> ImportArticleAsync(CancellationToken cancellationToken)
        {
            var article = new Article();

            var file = _contextAccessor.HttpContext.Request?.Form?.Files["id"];

            string title = _contextAccessor.HttpContext.Request?.Form["title"];

            string pic_url = _contextAccessor.HttpContext.Request?.Form["pic_url"];

            string pic_info = _contextAccessor.HttpContext.Request?.Form["pic_info"];

            var uploadFileBytes = new byte[file.Length];

            file.OpenReadStream().Read(uploadFileBytes, 0, (int)file.Length);

            string str = System.Text.Encoding.Default.GetString(uploadFileBytes);

            if (!string.IsNullOrWhiteSpace(str))
            {             
                article.Content = str;

                if (string.IsNullOrEmpty(title))
                {
                    article.Title = file.FileName.Split(".")[0];
                }
                else
                {
                    article.Title = title;
                }
                article.PicUrl = pic_url;
                article.PicInfo = pic_info;
            }

            await _blogRepository.AddArticleAsync(article, cancellationToken);

            return article;
        }
    }
}
