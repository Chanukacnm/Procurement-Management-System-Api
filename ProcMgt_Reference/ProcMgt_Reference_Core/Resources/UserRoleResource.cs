using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Core.Resources
{
    public class UserRoleResource
    {
        public Guid UserRoleID { get; set; }
        public string UserRoleName { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public bool? IsTansactions { get; set; } //--- Add By Nipuna Francisku
    }
}
