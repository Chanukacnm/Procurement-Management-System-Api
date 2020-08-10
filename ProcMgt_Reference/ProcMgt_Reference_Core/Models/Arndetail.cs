using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    [Table("ARNDetail")]
    public partial class Arndetail
    {
        [Column("ARNDetailID")]
        public Guid ArndetailId { get; set; }
        [Column("ARNHeaderID")]
        public Guid? ArnheaderId { get; set; }
        [Column("ItemID")]
        public Guid? ItemId { get; set; }
        public double? InvoiceQty { get; set; }
        public double? RecivedQty { get; set; }
        public double? RejectedQty { get; set; }
        public string Remark { get; set; }

        [ForeignKey("ArnheaderId")]
        [InverseProperty("Arndetail")]
        public virtual Arnheader Arnheader { get; set; }
        [ForeignKey("ItemId")]
        [InverseProperty("Arndetail")]
        public virtual Item Item { get; set; }
    }
}
