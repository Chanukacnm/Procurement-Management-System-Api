using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class TaxResource
    {
        public Guid TaxID { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public double Percentage { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? EntryDateTime { get; set; }
    }
}
