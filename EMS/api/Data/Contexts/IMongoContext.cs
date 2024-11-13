using MongoDB.Driver;

namespace api.Data.Contexts
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
    }
}
