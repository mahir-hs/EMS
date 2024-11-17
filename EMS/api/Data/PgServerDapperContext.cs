using System.Data;
using api.Data.Contexts;
using Npgsql;

namespace api.Data
{
    public class PgServerDapperContext : IPgContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PgServerDapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("PostgresSqlConnection")!;
        }
        public IDbConnection CreateConnection()
        {

            return new NpgsqlConnection(_connectionString);
        }
    }
}
