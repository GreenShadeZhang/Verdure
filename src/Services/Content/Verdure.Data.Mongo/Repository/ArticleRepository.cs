using MongoDB.Driver;
using System.Linq.Expressions;
using Verdure.Common;
using Verdure.Infrastructure;

namespace Verdure.Data.Mongo
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly MongoDbContext _context;

        public ArticleRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<Article> GetAsync(string id, CancellationToken cancellationToken)
        {
            var filter = Builders<Article>.Filter.Where(a => a.Id == id);

            var result = await _context.Articles.FindAsync(filter, null, cancellationToken);

            return result.FirstOrDefault();
        }

        public Task<IEnumerable<Article>> GetListAsync(QueryRequest request, CancellationToken cancellationToken)
        {
            Expression<Func<Article, bool>> expression = null;

            var query = _context.Articles.AsQueryable();

            if (request.KeyWord != null)
            {
                expression = a => a.Title.Contains(request.KeyWord) || a.Content.Contains(request.KeyWord);
            }

            if (expression != null)
            {
                query = (MongoDB.Driver.Linq.IMongoQueryable<Article>)query.Where(expression);

            }

            var list = query.Skip(request.PageIndex - 1).Take(request.PageSize);

            return Task.FromResult(list.AsEnumerable());
        }
    }
}
