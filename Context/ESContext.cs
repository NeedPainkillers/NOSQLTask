using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nest;
using NOSQLTask.Data;

namespace NOSQLTask.Context
{
    public class ESContext
    {
        public ESContext(IOptions<Settings> isettings)
        {
            //"http://myserver:9200"
            var node = new Uri(isettings.Value.ElascticConnectionString);
            var settings = new ConnectionSettings(node);
            Connection = new ElasticClient(settings);
        }

        public ElasticClient Connection { get; }
    }
}
