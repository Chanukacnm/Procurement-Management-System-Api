using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class ApprovalFlowManagementResource
    {
        public Guid ApprovalFlowManagementID { get; set; }
        public Guid ApprovalPatternTypeID { get; set; }
        public string ApprovalPatternName { get; set; }
        public string ApprovalSequenceNo { get; set; }
        public Guid DesignationID { get; set; }
        public string DesignationName { get; set; }

    }
}
