using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class StockResource
    {
        public Guid StockID { get; set; }
        public Guid ItemID { get; set; }
        public double StockQty { get; set; }
        public double BalancedQty { get; set; }
        public double ReceivedQty { get; set; }
    }
}
