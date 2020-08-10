using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Core.Resources
{
    public class MeasurementUnitResource
    {
        public Guid MeasurementUnitID { get; set; }
        public string MeasurementUnitName { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public bool? IsTansactions { get; set; } //--- Add By Nipuna Francisku

    }
}
