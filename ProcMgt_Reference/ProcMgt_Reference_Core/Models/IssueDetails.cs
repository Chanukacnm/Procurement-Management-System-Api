using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class IssueDetails
    {
        [Key]
        [Column("IssueDetailID")]
        public Guid IssueDetailId { get; set; }
        [Column("IssuedHeaderID")]
        public Guid IssuedHeaderId { get; set; }
        [Column("ItemID")]
        public Guid ItemId { get; set; }
        public double Qty { get; set; }

        [ForeignKey("IssuedHeaderId")]
        [InverseProperty("IssueDetails")]
        public virtual IssueHeader IssuedHeader { get; set; }
    }
}
