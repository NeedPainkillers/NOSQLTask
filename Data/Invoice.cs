using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;

namespace NOSQLTask.Data
{
    public class Invoice
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string InvoiceId { get; set; }
        public int ClientId { get; set; } = -1;
        public List<string> ProductIds { get; set; } = new List<string>();
        public bool Status { get; set; } = default;

    }
}
