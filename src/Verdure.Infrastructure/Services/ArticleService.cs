using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Verdure.Core;

namespace Verdure.Infrastructure
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;

        public ArticleService(IArticleRepository repository, IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
        }

        public Task<Article> AddAsync(Article article, CancellationToken cancellationToken)
        {
            return _repository.AddAsync(article, cancellationToken);
        }

        public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            return _repository.DeleteAsync(id, cancellationToken);
        }

        public Task<Article> GetAsync(string id, CancellationToken cancellationToken)
        {
            return _repository.GetAsync(id, cancellationToken);
        }

        public Task<IEnumerable<Article>> GetListAsync(QueryRequest request, CancellationToken cancellationToken)
        {
            return _repository.GetListAsync(request, cancellationToken);
        }

        public Task<Article> ImportArticleAsync(CancellationToken cancellationToken)
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
            }
            return _repository.ImportArticleAsync(article, cancellationToken);
        }

        public Task<Article> UpdateAsync(Article article, CancellationToken cancellationToken)
        {
            return _repository.UpdateAsync(article, cancellationToken);
        }
    }
}
