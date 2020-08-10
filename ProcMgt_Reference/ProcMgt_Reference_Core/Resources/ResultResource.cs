using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class ResultResource
    {
        public object ResultObject { get; set; }
        public string Message { get; set; } 
        public bool status { get; set; }
        public string Details { get; set; }

    }
}
