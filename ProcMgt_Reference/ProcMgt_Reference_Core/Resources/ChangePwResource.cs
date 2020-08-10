using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class ChangePwResource
    {
        public Guid UserID { get; set; }
        public string CurrentPw { get; set; }
        public string Password { get; set; }
    }
}
