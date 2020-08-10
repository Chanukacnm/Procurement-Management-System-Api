using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class DesignationBusinessUnitResource
    {
        public Guid DesignationBusinessUnitID { get; set; }
        public Guid DesignationID { get; set; }
        public Guid BusinessUnitTypeID { get; set; }
        public Guid? BusinessUnitsID { get; set; }
        public Guid? UserId { get; set; }
        public string DesignationName { get; set; }
        public string BusinessUnitTypeName { get; set; }
        public string BusinessUnitsName { get; set; }

    }
}
