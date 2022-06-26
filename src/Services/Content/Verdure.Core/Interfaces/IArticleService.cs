using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Verdure.Common;

namespace Verdure.Core
{
    public interface IArticleService
    {
        Task<Article> GetAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<Article>> GetListAsync(QueryRequest request, CancellationToken cancellationToken);
    }
}
