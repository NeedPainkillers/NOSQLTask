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

    class VisitLogEqualityComparer : IEqualityComparer<VisitLog>
    {
        public bool Equals(VisitLog p1, VisitLog p2)
        {
            if (p2 == null && p1 == null)
                return true;
            else if (p1 == null || p2 == null)
                return false;
            else if (p1.ClientId.Equals(p2.ClientId))
                return true;
            else
                return false;
        }

        public int GetHashCode(VisitLog pr)
        {
            var hCode = pr.ClientId;
            return hCode.GetHashCode();
        }
    }
}
