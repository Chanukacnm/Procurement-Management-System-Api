using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class QuotationRequestStatus
    {
        public QuotationRequestStatus()
        {
            QuotationRequestHeader = new HashSet<QuotationRequestHeader>();
        }

        [Column("QuotationRequestStatusID")]
        public int QuotationRequestStatusId { get; set; }
        [Required]
        [Column("QuotationRequestStatus")]
        [StringLength(50)]
        public string QuotationRequestStatus1 { get; set; }

        [InverseProperty("QuotationRequestStatus")]
        public virtual ICollection<QuotationRequestHeader> QuotationRequestHeader { get; set; }
    }
}
