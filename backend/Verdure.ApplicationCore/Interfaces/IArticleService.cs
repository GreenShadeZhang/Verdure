using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Verdure.ApplicationCore
{
    public interface IArticleService
    {
        Task<bool> AddAsync(Article article, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Article>> ListAsync(int pi = 1, int ps = 10, CancellationToken cancellationToken = default);
    }
}
