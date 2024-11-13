using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using api.Data.Contexts;

namespace api.Data
{
    public class SqlServerDapperContext : IDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;


        public SqlServerDapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlServerConnection")!;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}