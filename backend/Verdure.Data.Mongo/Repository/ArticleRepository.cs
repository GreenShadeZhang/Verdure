using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verdure.Core;
using Verdure.Infrastructure;

namespace Verdure.Data.Mongo
{
    public class ArticleRepository : IArticleRepository
    {
        public Task<Article> AddAsync(Article article, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Article> GetAsync(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Article>> GetListAsync(QueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Article> ImportArticleAsync(Article article, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Article> UpdateAsync(Article article, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
