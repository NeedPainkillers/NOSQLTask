using Microsoft.Extensions.Options;
using Neo4j.Driver.V1;
using NOSQLTask.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOSQLTask.Context
{
    public class Neo4jContext : IDisposable
    {
        private readonly IDriver _driver;

        public Neo4jContext(IOptions<Settings> settings)
        {
            _driver = GraphDatabase.Driver(settings.Value.Neo4jConnectionUrl, AuthTokens.Basic(settings.Value.Neo4jConnectionLogin, settings.Value.Neo4jConnectionPassword));
        }

        public ISession GetSession
        {
            get 
            {
                return _driver.Session();
            }
        }

        public void PrintGreeting(string message)
        {
            using (var session = _driver.Session())
            {
                string greeting = session.WriteTransaction(tx =>
                {
                    var result = tx.Run("CREATE (a:Greeting) " +
                                        "SET a.message = $message " +
                                        "RETURN a.message + ', from node ' + id(a)",
                        new { message });
                    return result.Single()[0].ToString();
                });
                Console.WriteLine(greeting);
            }
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }
    }
}
