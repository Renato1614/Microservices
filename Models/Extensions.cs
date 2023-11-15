using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Play.Catalog.Service.Settings;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Models
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var _servicesettings = configuration.GetSection(nameof(ServiceSettings))
                                            .Get<ServiceSettings>();
                var mongodbSettings = configuration.GetSection(nameof(MongoDbSettings))
                                                    .Get<MongoDbSettings>();
                var mongoClient = new MongoClient(mongodbSettings.ConnectionString);
                return mongoClient.GetDatabase(_servicesettings.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepósitory<T>(this IServiceCollection services,
                                                               string collectionsName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProcvider =>
            {
                var database = serviceProcvider
                                .GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionsName);
            });

            return services;
        }
    }
}
