using Microsoft.Extensions.Options;
using NOSQLTask.Data;
using StackExchange.Redis;
using System;

namespace NOSQLTask.Context
{
    public class RedisContext : IDisposable
    {
        public RedisContext(IOptions<Settings> settings)
        {
            Connection = ConnectionMultiplexer.Connect(settings.Value.RedisConnectionString); //localhost possibly
        }

        public ConnectionMultiplexer Connection { get; }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
