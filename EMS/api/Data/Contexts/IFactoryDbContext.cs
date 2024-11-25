using System.Data;
using MongoDB.Driver;

namespace api.Data.Contexts
{
    public interface IFactoryDbContext
    {
        IDbConnection SqlConnection();
        IDbConnection PgConnection();
        IMongoDatabase MongoConnection { get; }
    }
}
