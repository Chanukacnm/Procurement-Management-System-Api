using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Core.Resources
{
    public class DepartmentResource
    {
        public string DepartmentName { get; set; }
        public string CompanyName { get; set; }
        public bool? IsTansactions { get; set; } //--- Add By Nipuna Francisku

        public Guid DepartmentID { get; set; }
        
        public Guid CompanyID { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }


    }
}
