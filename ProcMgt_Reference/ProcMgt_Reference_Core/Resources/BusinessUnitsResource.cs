using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class BusinessUnitsResource
    {
        public Guid BusinessUnitsID { get; set; }
        public string BusinessUnitsName { get; set; }
        public Guid BusinessUnitTypeID { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public DateTime? EntryDateTime { get; set; }
    }
}
