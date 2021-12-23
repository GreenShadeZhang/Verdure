using MongoDB.Driver;
using System.Linq.Expressions;
using Verdure.Admin.Data.Mongo;
using Verdure.Admin.Infrastructure;
using Verdure.Common;

namespace Verdure.Admin.Data.Mongo
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MongoDbContext _context;

        public AdminRepository(MongoDbContext context)
        {
            _context = context;
        }
        public async Task<Article> AddAsync(Article article, CancellationToken cancellationToken)
        {
            await _context.Articles.InsertOneAsync(article, null, cancellationToken);

            return article;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var ret = false;

            var filter = Builders<Article>.Filter.Where(a => a.Id == id);

            var result = await _context.Articles.DeleteOneAsync(filter, cancellationToken);

            if (result.DeletedCount > 0)
            {
                ret = true;
            }
            return ret;
        }

        public async Task<Article> GetByTitleAsync(string title, CancellationToken cancellationToken)
        {
            var filter = Builders<Article>.Filter.Where(a => a.Title == title);

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

        public async Task<Article> ImportArticleAsync(Article article, CancellationToken cancellationToken)
        {
            var filter = Builders<Article>.Filter.Where(a => a.Id == article.Id);

            await _context.Articles.ReplaceOneAsync(filter, article, new ReplaceOptions { IsUpsert = true }, cancellationToken);

            return article;
        }

        public async Task<Article> UpdateAsync(Article article, CancellationToken cancellationToken)
        {
            var filter = Builders<Article>.Filter.Where(a => a.Id == article.Id);

            await _context.Articles.ReplaceOneAsync(filter, article, new ReplaceOptions { IsUpsert = false }, cancellationToken);

            return article;
        }
    }
}
