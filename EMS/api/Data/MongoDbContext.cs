using api.Data.Contexts;
using MongoDB.Driver;

namespace api.Data
{
    public class MongoDbContext : IMongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var databaseName = configuration.GetValue<string>("MongoDb:DatabaseName");

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
        public IMongoDatabase Database => _database;
    }
}
