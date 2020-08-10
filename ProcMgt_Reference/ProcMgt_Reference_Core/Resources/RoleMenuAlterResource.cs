using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
   public class RoleMenuAlterResource
    {

        public Guid UserRoleID { get; set; }
        public IEnumerable<int> MenuID { get; set; }

        //public Menu[] Menu { get; set; }



    }
}
