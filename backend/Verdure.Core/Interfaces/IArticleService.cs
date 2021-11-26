using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Verdure.Core
{
    public interface IArticleService
    {
        Task<Article> AddAsync(Article article, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
        Task<Article> UpdateAsync(Article article, CancellationToken cancellationToken);
        Task<IEnumerable<Article>> GetListAsync(QueryRequest request, CancellationToken cancellationToken);
        Task<Article> ImportArticleAsync(CancellationToken cancellationToken);
    }
}
