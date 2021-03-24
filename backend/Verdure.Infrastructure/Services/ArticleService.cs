using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Verdure.ApplicationCore;

namespace Verdure.Infrastructure
{
    public class ArticleService : IArticleService
    {
        private readonly IBlogRepository _repository;

        public ArticleService(IBlogRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddAsync(Article article, CancellationToken cancellationToken)
        {
            await _repository.AddArticleAsync(article, cancellationToken);

            return true;
        }

        public async Task<IReadOnlyCollection<Article>> ListAsync(int pi = 1, int ps = 10, CancellationToken cancellationToken = default)
        {
            var list = _repository.Articles.Skip((pi - 1) * ps).Take(ps).ToList();

            return list;
        }
    }
}
