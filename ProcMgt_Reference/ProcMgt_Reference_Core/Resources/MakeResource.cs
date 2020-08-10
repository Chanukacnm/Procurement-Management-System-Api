using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Core.Resources
{
    public class MakeResource
    {
        public string ItemTypeName { get; set; } //temporarily used
        public Guid MakeID { get; set; }
        public Guid ItemTypeID { get; set; }
        public string MakeName { get; set; }
        public string MakeCode { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public bool? IsTansactions { get; set; } //--- Add By Nipuna Francisku

        //public string ItemTypeName { get; set; } //temporarily used

    }

}
