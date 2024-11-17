using System.Data;

namespace api.Data.Contexts
{
    public interface IPgContext
    {
        IDbConnection CreateConnection();
    }
}
