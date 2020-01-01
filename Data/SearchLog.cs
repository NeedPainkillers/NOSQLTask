using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOSQLTask.Data
{
    public class SearchLog
    {
        public int ClientId { get; set; } = -1;
        public string SearchBody { get; set; } = string.Empty;
    }
}
