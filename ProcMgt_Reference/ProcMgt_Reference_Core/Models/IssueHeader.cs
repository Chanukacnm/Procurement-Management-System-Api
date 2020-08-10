using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class IssueHeader
    {
        public IssueHeader()
        {
            IssueDetails = new HashSet<IssueDetails>();
        }

        [Column("IssuedHeaderID")]
        public Guid IssuedHeaderId { get; set; }
        [Column("ItemRequestID")]
        public Guid ItemRequestId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime IssuedDateTime { get; set; }
        [Column("IssuedUserID")]
        public Guid IssuedUserId { get; set; }
        [StringLength(250)]
        public string Comment { get; set; }

        [ForeignKey("IssuedUserId")]
        [InverseProperty("IssueHeader")]
        public virtual User IssuedUser { get; set; }
        [InverseProperty("IssuedHeader")]
        public virtual ICollection<IssueDetails> IssueDetails { get; set; }
    }
}
