using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Play.common.Settings;

namespace Play.common.MongoDB
{
    public static class Extentions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            //            var configuration = services.get.Configuration;
            //            ServiceSettings serviceSettings = configuration.GetSection(nameof(ServiceSettings))
            //.Get<ServiceSettings>();
            services.AddSingleton(ServiceProvider =>
            {
                var configuration= ServiceProvider.GetService<IConfiguration>();
                var mongoDbSettings = configuration.GetSection(nameof(MongoDbsettings)).Get<MongoDbsettings>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });
            return services;
        }
        public static IServiceCollection AddMongoReposiory<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(ServiceProvider =>
             {
                 var database = ServiceProvider.GetService<IMongoDatabase>();
                 return new MongoRepository<T>(database, collectionName);
             });
            return services;
        }
    }
}
