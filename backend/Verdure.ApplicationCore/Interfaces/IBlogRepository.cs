using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Verdure.ApplicationCore
{
    public interface IBlogRepository
    {
        public IQueryable<Article> Articles { get; }
        Task AddArticleAsync(Article entity, CancellationToken cancellationToken);
    }
}
