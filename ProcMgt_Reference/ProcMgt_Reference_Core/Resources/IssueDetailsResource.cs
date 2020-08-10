using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class IssueDetailsResource
    {
        public Guid IssueDetailID { get; set; }
        public Guid IssuedHeaderID { get; set; }
        public Guid ItemID { get; set; }
        public double Qty { get; set; }
    }
}
