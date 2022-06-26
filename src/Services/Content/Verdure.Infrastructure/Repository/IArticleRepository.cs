using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Verdure.Common;

namespace Verdure.Infrastructure
{
    public interface IArticleRepository
    {
        Task<Article> GetAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<Article>> GetListAsync(QueryRequest request, CancellationToken cancellationToken);
    }
}
