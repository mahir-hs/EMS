using System.Data;
using api.Data.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Npgsql;

namespace api.Data
{
    public class FactoryDbContext : IFactoryDbContext
    {
        private readonly IConfiguration _sqlConfiguration, _pgConfiguration;
        private readonly string _sqlConnectionString, _pgConnectionString;
        private readonly IMongoDatabase _mongoConnection;
        public FactoryDbContext(IConfiguration sqlConfiguration, IConfiguration pgConfiguration, IConfiguration mongoConfiguration) 
        {
            _sqlConfiguration = sqlConfiguration;
            _sqlConnectionString = _sqlConfiguration.GetConnectionString("SqlServerConnection")!;
            _pgConfiguration = pgConfiguration;
            _pgConnectionString = _pgConfiguration.GetConnectionString("PostgresSqlConnection")!;

            var mongoConnectionString = mongoConfiguration.GetValue<string>("MongoDb:ConnectionString");
            var databaseName = mongoConfiguration.GetValue<string>("MongoDb:DatabaseName");

            var client = new MongoClient(mongoConnectionString);
            _mongoConnection = client.GetDatabase(databaseName);
        }



        public IMongoDatabase MongoConnection => _mongoConnection;

        public IDbConnection PgConnection()
        {
            return new NpgsqlConnection(_pgConnectionString);
        }

        public IDbConnection SqlConnection()
        {
            return new SqlConnection(_sqlConnectionString);
        }
    }
}
