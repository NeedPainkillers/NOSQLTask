using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOSQLTask.Data
{
    public class ProductNeo4j
    {
        public string ProductId { get; set; } = string.Empty;
    }
    public class Product : ProductNeo4j
    {
        public string Specification { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Quantity { get; set; } = -1;
        public int Price { get; set; } = -1;
    }

}
