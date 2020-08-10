using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
   public class RoleMenuResource
    {

        public int RoleMenuID { get; set; }
        public int MenuID { get; set; }
        public Guid UserRoleID { get; set; }
    }
}
