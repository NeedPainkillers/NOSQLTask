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
    class ProductEqualityComparer : IEqualityComparer<Product>
    {
        public bool Equals(Product p1, Product p2)
        {
            if (p2 == null && p1 == null)
                return true;
            else if (p1 == null || p2 == null)
                return false;
            else if (p1.ProductId.Equals(p2.ProductId))
                return true;
            else
                return false;
        }

        public int GetHashCode(Product pr)
        {
            var hCode = pr.ProductId;
            return hCode.GetHashCode();
        }
    }

}
