using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verdure.Common;

namespace Verdure.Admin.Core
{
    public interface IAdminService
    {
        Task<Article> AddAsync(Article article, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
        Task<Article> UpdateAsync(Article article, CancellationToken cancellationToken);
        Task<Article> ImportArticleAsync(CancellationToken cancellationToken);
    }
}
