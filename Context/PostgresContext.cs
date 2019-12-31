using Microsoft.Extensions.Options;
using NOSQLTask.Data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOSQLTask.Context
{
    public class PostgresContext
    {
        string _connString = "Host=host.docker.internal;Username=postgres;Password=sdfl234;Database=taskdb";
        //private readonly NpgsqlConnection _conn = null;

        public PostgresContext(IOptions<Settings> settings)
        {
            //_conn = new NpgsqlConnection(settings.Value.PostgresConnectionString);
            _connString = settings.Value.PostgresConnectionString;
        }


        public NpgsqlConnection GetConnection
        {
            get
            {
                return new NpgsqlConnection(_connString);
            }
        }

        //public void Dispose()
        //{
        //    //_conn.CloseAsync();
        //    //_conn.Dispose();
        //}
    }
}
