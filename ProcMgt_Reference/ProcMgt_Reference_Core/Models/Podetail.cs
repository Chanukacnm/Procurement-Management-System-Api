using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    [Table("PODetail")]
    public partial class Podetail
    {
        [Column("PODetailID")]
        public Guid PodetailId { get; set; }
        [Column("POHeaderID")]
        public Guid PoheaderId { get; set; }
        [Column("ItemID")]
        public Guid ItemId { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public double Qty { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "money")]
        public decimal? DiscountAmount { get; set; }

        [ForeignKey("ItemId")]
        [InverseProperty("Podetail")]
        public virtual Item Item { get; set; }
        [ForeignKey("PoheaderId")]
        [InverseProperty("Podetail")]
        public virtual Poheader Poheader { get; set; }
    }
}
