using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class BusinessUnitTypeResource
    {
        public Guid BusinessUnitTypeID { get; set; }
        public string BusinessUnitTypeName { get; set; }
        public Guid DesignationID { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public DateTime? EntryDateTime { get; set; }
    }
}
