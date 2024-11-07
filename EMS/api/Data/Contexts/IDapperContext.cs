using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace api.Data.Contexts
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}