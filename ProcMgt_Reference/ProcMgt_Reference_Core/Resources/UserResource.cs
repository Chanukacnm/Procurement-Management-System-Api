using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class UserResource
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public Guid CompanyID { get; set; }
        public Guid DepartmentID { get; set; }
        public string Email { get; set; }
        public string DesignationName { get; set; }
        public Guid DesignationID { get; set; }
        public Guid ApprDesignationID { get; set; }
        public string UserRoleName { get; set; }
        public Guid UserRoleID { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public bool? IsApprovalUser { get; set; }
        public bool? IsTansactions { get; set; } //--- Add By Nipuna Francisku

    }
}
