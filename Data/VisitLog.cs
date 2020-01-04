using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOSQLTask.Data
{
    public class VisitLog
    {
        public int ClientId { get; set; } = -1;
        public string FromProduct { get; set; } = string.Empty;
        public string ToProduct { get; set; } = string.Empty;
    }
}
