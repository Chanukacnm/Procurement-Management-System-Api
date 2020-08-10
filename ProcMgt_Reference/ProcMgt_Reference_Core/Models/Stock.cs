using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Stock
    {
        [Column("StockID")]
        public Guid StockId { get; set; }
        [Column("ItemID")]
        public Guid ItemId { get; set; }
        public double StockQty { get; set; }
        public double BalancedQty { get; set; }
        public double ReceivedQty { get; set; }

        [ForeignKey("ItemId")]
        [InverseProperty("Stock")]
        public virtual Item Item { get; set; }
    }
}
