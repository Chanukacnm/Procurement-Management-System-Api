using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class IssueHeaderResource
    {
        public Guid IssuedHeaderID { get; set; }
        public Guid ItemRequestID { get; set; }
        public DateTime IssuedDateTime { get; set; }
        public Guid IssuedUserID { get; set; }
        public string Comment { get; set; }

        public double ReceivedQty { get; set; }

        public IEnumerable<IssueDetails> IssueDetails { get; set; }


    }
}
