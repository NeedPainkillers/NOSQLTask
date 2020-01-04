using Microsoft.Extensions.Options;
using Neo4j.Driver.V1;
using Neo4jClient;
using Neo4jClient.Cypher;
using NOSQLTask.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOSQLTask.Context
{
    public class Neo4jContext : IDisposable
    {
        //private readonly IDriver _driver;
        private readonly GraphClient _client;

        public Neo4jContext(IOptions<Settings> settings)
        {
            //_driver = GraphDatabase.Driver(settings.Value.Neo4jConnectionUrl, AuthTokens.Basic(settings.Value.Neo4jConnectionLogin, settings.Value.Neo4jConnectionPassword));
            Uri uri = new Uri(settings.Value.Neo4jConnectionUrl);
            _client = new GraphClient(uri, settings.Value.Neo4jConnectionLogin, settings.Value.Neo4jConnectionPassword);
        }

        //public ISession GetSession
        //{
        //    get 
        //    {
        //        return _driver.Session();
        //    }
        //}

        public async Task<bool> Connect()
        {
            if(!_client.IsConnected)
            {
                await _client.ConnectAsync();
            }
            return _client.IsConnected;
        }

        public ICypherFluentQuery GetCypher
        {
            get
            {
                return _client.Cypher;
            }
        }

        public void Dispose()
        {
            //_driver?.Dispose();
            _client?.Dispose();
        }
    }
}
