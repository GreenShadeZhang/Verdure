using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Verdure.Core
{
    public interface IBlogManageService
    {
        public Task<Article> ImportArticleAsync(CancellationToken cancellationToken);
    }
}
