using MongoDB.Driver;

namespace Verdure.Infrastructure
{
    public class MongoDBConfiguration
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public SslSettings SslSettings { get; set; }
    }
}
