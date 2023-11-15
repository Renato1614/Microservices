using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Play.Catalog.Service.Models;
using System.Collections;

namespace Play.Catalog.Service.Db
{
    public class DbBuilder<T> where T : class
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        public DbBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("Mongo");
        }

        public IMongoCollection<T> Start(string databaseName, string collectionName)
        {
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase(databaseName);
            return database.GetCollection<T>(collectionName);
        }
    }
}
