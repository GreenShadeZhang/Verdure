using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Verdure.Common;
using Verdure.Core;

namespace Verdure.Infrastructure
{
    public interface IArticleRepository
    {
        Task<Article> AddAsync(Article article, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
        Task<Article> UpdateAsync(Article article, CancellationToken cancellationToken);
        Task<Article> GetAsync(string id, CancellationToken cancellationToken);
        Task<Article> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<IEnumerable<Article>> GetListAsync(QueryRequest request, CancellationToken cancellationToken);
        Task<Article> ImportArticleAsync(Article article, CancellationToken cancellationToken);
    }
}
