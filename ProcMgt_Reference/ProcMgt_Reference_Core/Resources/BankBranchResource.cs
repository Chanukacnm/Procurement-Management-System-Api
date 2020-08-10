using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class BankBranchResource
    {
        public Guid BranchID { get; set; }
        public string BranchName { get; set; }
        public Guid BankID { get; set; }
    }
}
