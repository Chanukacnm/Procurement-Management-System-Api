using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class ReportResource
    {
        public Guid ReportID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
