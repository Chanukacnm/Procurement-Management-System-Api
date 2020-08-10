using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    [Table("POHeader")]
    public partial class Poheader
    {
        public Poheader()
        {
            Arnheader = new HashSet<Arnheader>();
            Podetail = new HashSet<Podetail>();
        }

        [Column("POHeaderID")]
        public Guid PoheaderId { get; set; }
        [Column("QuotationRequestHeaderID")]
        public Guid QuotationRequestHeaderId { get; set; }
        [Required]
        [Column("PONumber")]
        [StringLength(50)]
        public string Ponumber { get; set; }
        [Column("TotalPOAmount")]
        public int? TotalPoamount { get; set; }
        [Column(TypeName = "date")]
        public DateTime? RequestedDeliveryDate { get; set; }
        [Column("PODateTime", TypeName = "datetime")]
        public DateTime PodateTime { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        public bool IsDeliver { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActualDeliveryDate { get; set; }
        [Column("PaymentMethodID")]
        public Guid? PaymentMethodId { get; set; }
        [Column(TypeName = "money")]
        public decimal? TaxAmount { get; set; }
        public bool IsEnter { get; set; }

        [ForeignKey("PaymentMethodId")]
        [InverseProperty("Poheader")]
        public virtual PaymentMethod PaymentMethod { get; set; }
        [ForeignKey("QuotationRequestHeaderId")]
        [InverseProperty("Poheader")]
        public virtual QuotationRequestHeader QuotationRequestHeader { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Poheader")]
        public virtual User User { get; set; }
        [InverseProperty("Poheader")]
        public virtual ICollection<Arnheader> Arnheader { get; set; }
        [InverseProperty("Poheader")]
        public virtual ICollection<Podetail> Podetail { get; set; }
    }
}
