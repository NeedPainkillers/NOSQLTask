using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;

namespace NOSQLTask.Data
{
    public class Invoice
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string InvoiceId { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public List<string> ProductIds { get; set; } = new List<string>();
        public bool Status { get; set; } = default;

    }
}
