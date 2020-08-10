using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class ApprovalPatternType
    {
        public ApprovalPatternType()
        {
            ApprovalFlowManagement = new HashSet<ApprovalFlowManagement>();
            ItemType = new HashSet<ItemType>();
        }

        [Column("ApprovalPatternTypeID")]
        public Guid ApprovalPatternTypeId { get; set; }
        [Required]
        [StringLength(30)]
        public string PatternName { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        public bool? IsActive { get; set; }

        [InverseProperty("ApprovalPatternType")]
        public virtual ICollection<ApprovalFlowManagement> ApprovalFlowManagement { get; set; }
        [InverseProperty("ApprovalPatternType")]
        public virtual ICollection<ItemType> ItemType { get; set; }
    }
}
