using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Core.Resources
{
    public class ApprovalPatternTypeResource
    {
        public Guid ApprovalPatternTypeID { get; set; }
        public string PatternName { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
    }
}
