using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
   public class ModuleMenuResource
    
    {
        public int ModuleMenuID { get; set; }
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public int MenuID { get; set; }
        public string MenuIDHTML { get; set; }
        public string MenuName { get; set; }
        public string URL { get; set; }
        public string Icon { get; set; }

        //public IEnumerable<MenuResource> MenuDetails { get; set; }
    }
}
