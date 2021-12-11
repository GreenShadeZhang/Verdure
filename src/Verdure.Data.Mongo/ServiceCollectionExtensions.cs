using Microsoft.Extensions.DependencyInjection;

namespace Verdure.Data.Mongo
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseMongoDbPersistence(this IServiceCollection service, Action<MongoDbOptions> configureOptions)
        {
            service.Configure(configureOptions);
            service.AddSingleton<MongoDbContext>();
            return service;
        }
    }
}
