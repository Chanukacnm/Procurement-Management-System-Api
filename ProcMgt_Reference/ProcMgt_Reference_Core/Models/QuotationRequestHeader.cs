using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class QuotationRequestHeader
    {
        public QuotationRequestHeader()
        {
            Poheader = new HashSet<Poheader>();
            QuotationRequestDetails = new HashSet<QuotationRequestDetails>();
        }

        [Column("QuotationRequestHeaderID")]
        public Guid QuotationRequestHeaderId { get; set; }
        [Column("SupplierID")]
        public Guid? SupplierId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime QuotationRequestedDate { get; set; }
        [Required]
        [StringLength(200)]
        public string QuotationNumber { get; set; }
        [Column("UserID")]
        public Guid UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RequiredDate { get; set; }
        [StringLength(200)]
        public string ApprovalComment { get; set; }
        [Column("QuotationRequestStatusID")]
        public int QuotationRequestStatusId { get; set; }
        [Required]
        public bool? IsEnteringCompleted { get; set; }
        public bool? IsDelivered { get; set; }

        [ForeignKey("QuotationRequestStatusId")]
        [InverseProperty("QuotationRequestHeader")]
        public virtual QuotationRequestStatus QuotationRequestStatus { get; set; }
        [InverseProperty("QuotationRequestHeader")]
        public virtual ICollection<Poheader> Poheader { get; set; }
        [InverseProperty("QuotationRequestHeader")]
        public virtual ICollection<QuotationRequestDetails> QuotationRequestDetails { get; set; }
    }
}
