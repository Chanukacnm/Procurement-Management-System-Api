using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcMgt_Reference_Core.Models
{
    public partial class ApprovalFlowManagement
    {
        [Column("ApprovalFlowManagementID")]
        public Guid ApprovalFlowManagementId { get; set; }
        [Column("ApprovalPatternTypeID")]
        public Guid ApprovalPatternTypeId { get; set; }
        [StringLength(50)]
        public string ApprovalSequenceNo { get; set; }
        [Column("DesignationID")]
        public Guid DesignationId { get; set; }

        [ForeignKey("ApprovalPatternTypeId")]
        [InverseProperty("ApprovalFlowManagement")]
        public virtual ApprovalPatternType ApprovalPatternType { get; set; }
        [ForeignKey("DesignationId")]
        [InverseProperty("ApprovalFlowManagement")]
        public virtual Designation Designation { get; set; }
    }
}
