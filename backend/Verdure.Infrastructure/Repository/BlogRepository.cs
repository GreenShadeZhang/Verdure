using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Verdure.ApplicationCore;

namespace Verdure.Infrastructure
{
    public class BlogRepository : MongoDBContextBase, IBlogRepository
    {
        private readonly IMongoCollection<Article> _articles;
        public BlogRepository(IOptions<MongoDBConfiguration> settings)
            : base(settings)
        {
            _articles = Database.GetCollection<Article>(Constants.TableNames.Blogs);
            CreateBlogsIndexes();
        }

        private void CreateBlogsIndexes()
        {
            var indexOptions = new CreateIndexOptions() { Background = true };

            var builder = Builders<Article>.IndexKeys;
            var clientIdIndexModel = new CreateIndexModel<Article>(builder.Ascending(_ => _.Title), indexOptions);
            _articles.Indexes.CreateOne(clientIdIndexModel);
        }

        public IQueryable<Article> Articles => _articles.AsQueryable();

        public async Task AddArticleAsync(Article entity, CancellationToken cancellationToken)
        {
            await _articles.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }
    }
}
