using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOSQLTask.Data
{
    public class Product
    {
        public int ProductId { get; set; } = -1;
        public string Specification { get; set; } = string.Empty;
        public int Quantity { get; set; } = -1;
        public int Price { get; set; } = -1;
    }
}
