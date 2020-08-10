using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class UploadFileResource
    {
        public Guid UploadFileID { get; set; }
        public string UploadFileName { get; set; }
        public string UploadFilePath { get; set; }
        public string FileExtension { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateTime { get; set; }
    }
}
