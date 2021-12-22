using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Verdure.Common;
using Verdure.Core;

namespace Verdure.Data.Mongo
{
    public class MongoDbContext
    {
        public MongoDbContext(IOptions<MongoDbOptions> options)
        {
            var connectionString = options.Value.ConnectionString;
            var mongoClient = new MongoClient(connectionString);
            var databaseName = options.Value.DatabaseName is not null and not "" ? options.Value.DatabaseName : MongoUrl.Create(connectionString).DatabaseName;

            if (databaseName == null)
                throw new Exception("Please specify a database name, either via the connection string or via the DatabaseName setting.");

            MongoDatabase = mongoClient.GetDatabase(databaseName);
        }

        protected IMongoDatabase MongoDatabase { get; }

        public IMongoCollection<Article> Articles => MongoDatabase.GetCollection<Article>("Article");
    }
}
