using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Verdure.Common;
using Verdure.Core;

namespace Verdure.Infrastructure
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repository;

        public ArticleService(IArticleRepository repository)
        {
            _repository = repository;
        }

        public Task<Article> GetAsync(string id, CancellationToken cancellationToken)
        {
            return _repository.GetAsync(id, cancellationToken);
        }

        public Task<IEnumerable<Article>> GetListAsync(QueryRequest request, CancellationToken cancellationToken)
        {
            return _repository.GetListAsync(request, cancellationToken);
        }
    }
}
