using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Verdure.Core
{
    public interface IBlogRepository
    {
        public IQueryable<Article> Articles { get; }
        Task AddArticleAsync(Article entity, CancellationToken cancellationToken);
    }
}
