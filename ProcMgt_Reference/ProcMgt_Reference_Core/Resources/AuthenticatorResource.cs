using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class AuthenticatorResource
    {
        public User UserDetails { get; set; }
        public IEnumerable<MenuResource> MenuDetails { get; set; }
        public IEnumerable<ModuleResource> ModuleDetails { get; set; }
        public IEnumerable<ModuleMenuResource> ModuleMenuDetails { get; set; }
        public String Token { get; set; }
        

    }
}
