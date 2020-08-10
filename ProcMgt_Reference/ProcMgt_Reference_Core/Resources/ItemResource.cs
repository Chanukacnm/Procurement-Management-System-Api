using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class ItemResource
    {
        public Guid ItemID { get; set; }
        public Guid ItemTypeID{ get; set; }
        public string ItemTypeName { get; set; }
        public string ItemDescription { get; set; }
        public string ItemCode { get; set; }
        public double? CurrentQty { get; set; }
        public double StockQty { get; set; }
        public double InitialQty { get; set; }
        public bool IsActive { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? EntryDateTime { get; set; }

        public double? ReOrderQuantity { get; set; }
        public string Status { get; set; }
        public bool? IsTansactions { get; set; } //--- Add By Nipuna Francisku

    }

}
