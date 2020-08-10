using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Core.Resources
{
    public class ModelResource
    {
        public Guid ModelID { get; set; }
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public string MakeName { get; set; }
        public Guid MakeID { get; set; }
        public string ItemTypeName { get; set; }
        public Guid ItemTypeID { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public string UploadFileName { get; set; }
        public string Image { get; set; }
        public Guid? UploadFileID { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public bool? IsTansactions { get; set; } //--- Add By Nipuna Francisku




    }
}
