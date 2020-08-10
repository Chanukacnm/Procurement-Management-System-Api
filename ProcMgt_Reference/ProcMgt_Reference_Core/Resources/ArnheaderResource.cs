using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class ArnheaderResource
    {
        public Guid ArnheaderID { get; set; }
        public Guid PoheaderID { get; set; }
        public string Arnnumber { get; set; }
        public DateTime RecivedDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceAtt { get; set; }
        public string Arnremark { get; set; }

        public virtual IEnumerable<Arndetail> Arndetail { get; set; }
        public virtual IEnumerable<ArndetailResource> ArndetailResource { get; set; }





    }
}
