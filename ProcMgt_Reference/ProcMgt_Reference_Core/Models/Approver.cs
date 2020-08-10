using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class Approver
    {
        [Column("ApproverID")]
        public Guid ApproverId { get; set; }
        [Required]
        [StringLength(100)]
        public string ApproverName { get; set; }
        [Column("UserID")]
        public Guid? UserId { get; set; }
        [Column("CategoryID")]
        public Guid? CategoryId { get; set; }
        [Required]
        public bool? IsActive { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Approver")]
        public virtual Category Category { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Approver")]
        public virtual User User { get; set; }
    }
}
