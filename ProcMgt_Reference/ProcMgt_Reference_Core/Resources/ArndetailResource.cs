using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
  public  class ArndetailResource
    {
        public Guid ArndetailID { get; set; }
        public Guid? ArnheaderID { get; set; }
        public Guid? ItemID { get; set; }
        public string ItemDescription { get; set; }
        public double? InvoiceQty { get; set; }
        public double? RecivedQty { get; set; }
        public double? RejectedQty { get; set; }
        public string Remark { get; set; }

       

        public double Qty { get; set; }
        
    }
}
