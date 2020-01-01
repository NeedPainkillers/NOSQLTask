using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOSQLTask.Data
{
    public class Settings
    {
        public string MongoConnectionString { get; set; }
        public string MongoDatabase { get; set; }
        public string RedisConnectionString { get; set; }
        public string PostgresConnectionString { get; set; }
        public string ElascticConnectionString { get; set; }
        public string Neo4jConnectionUrl { get; set; }
        public string Neo4jConnectionLogin { get; set; }
        public string Neo4jConnectionPassword { get; set; }
    }
}
