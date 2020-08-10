using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class QuotationRequestDetails
    {
        [Key]
        [Column("QuotationRequestDetailID")]
        public Guid QuotationRequestDetailId { get; set; }
        [Column("QuotationRequestHeaderID")]
        public Guid QuotationRequestHeaderId { get; set; }
        [Column("MakeID")]
        public Guid? MakeId { get; set; }
        [Column("ModelID")]
        public Guid? ModelId { get; set; }
        [Column("ItemID")]
        public Guid ItemId { get; set; }
        [Column("MeasurementUnitID")]
        public Guid MeasurementUnitId { get; set; }
        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }
        public double? Quantity { get; set; }
        [Column(TypeName = "money")]
        public decimal? GrossAmount { get; set; }
        [Column(TypeName = "money")]
        public decimal? NetAmount { get; set; }
        [Column(TypeName = "money")]
        public decimal? DiscountAmount { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? QuotationValidDate { get; set; }
        public string Attachment { get; set; }

        [ForeignKey("ItemId")]
        [InverseProperty("QuotationRequestDetails")]
        public virtual Item Item { get; set; }
        [ForeignKey("MakeId")]
        [InverseProperty("QuotationRequestDetails")]
        public virtual Make Make { get; set; }
        [ForeignKey("MeasurementUnitId")]
        [InverseProperty("QuotationRequestDetails")]
        public virtual MeasurementUnits MeasurementUnit { get; set; }
        [ForeignKey("ModelId")]
        [InverseProperty("QuotationRequestDetails")]
        public virtual Model Model { get; set; }
        [ForeignKey("QuotationRequestHeaderId")]
        [InverseProperty("QuotationRequestDetails")]
        public virtual QuotationRequestHeader QuotationRequestHeader { get; set; }
    }
}
