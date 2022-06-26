using Microsoft.AspNetCore.Http;
using Verdure.Admin.Core;
using Verdure.Admin.Infrastructure;
using Verdure.Common;

namespace Verdure.Admin.Infrastructure
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IIdGenerator _idGenerator;

        public AdminService(IAdminRepository repository, IHttpContextAccessor contextAccessor, IIdGenerator idGenerator)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
            _idGenerator = idGenerator;
        }

        public async Task<Article> AddAsync(Article article, CancellationToken cancellationToken)
        {
            article.Id = _idGenerator.Generate();

            var ret = await _repository.GetByTitleAsync(article.Title, cancellationToken);

            if (ret != null && !string.IsNullOrEmpty(ret.Title))
            {
                return article;
            }

            return await _repository.AddAsync(article, cancellationToken);
        }

        public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            return _repository.DeleteAsync(id, cancellationToken);
        }
        public Task<Article> ImportArticleAsync(CancellationToken cancellationToken)
        {
            var article = new Article
            {
                Id = _idGenerator.Generate()
            };

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
