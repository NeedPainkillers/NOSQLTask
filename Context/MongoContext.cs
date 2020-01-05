using Microsoft.Extensions.Options;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using NOSQLTask.Data;

namespace NOSQLTask.Context
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.MongoConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.MongoDatabase);
        }

        public IMongoCollection<Invoice> Invoices
        {
            get
            {
                return _database.GetCollection<Invoice>("Invoices");
            }
        }
    }
}
