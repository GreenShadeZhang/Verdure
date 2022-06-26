using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verdure.Common;

namespace Verdure.Admin.Infrastructure
{
    public interface IAdminRepository
    {
        Task<Article> AddAsync(Article article, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
        Task<Article> UpdateAsync(Article article, CancellationToken cancellationToken);
        Task<Article> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<Article> ImportArticleAsync(Article article, CancellationToken cancellationToken);
    }
}
