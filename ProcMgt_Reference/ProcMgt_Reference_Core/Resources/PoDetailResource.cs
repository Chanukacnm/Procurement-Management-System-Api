using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
   public class PoDetailResource
    {
        public Guid PodetailID { get; set; }
        public Guid PoheaderID { get; set; }
        public Guid ItemID { get; set; }
        public decimal UnitPrice { get; set; }
        public double Qty { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? DiscountAmount { get; set; }

        public string ItemDescription { get; set; }


        public double? InvoiceQty { get; set; }
        public double? RecivedQty { get; set; }
        public double? RejectedQty { get; set; }
        public string Remark { get; set; }

    } 
}
 