using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class ContactDetailsResource
    {
        public Guid ContactDetailsID { get; set; }
        public string ContactName { get; set; }
        public double ContactMobile { get; set; }
        public string Email { get; set; }
        public bool IsDefault { get; set; }
        public Guid SupplierID { get; set; }
        public string Default { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? EntryDateTime { get; set; }
    }
}
