using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
 public class PoHeaderResource
    {
        public string Ponumber { get; set; }
        //public double Qty { get; set; }

        public string UserName { get; set; }
        public string PaymentMethodName { get; set; }
        public string QuotationNumber { get; set; }
        //public string ItemDescription { get; set; }

        public Guid PoheaderID { get; set; }
        public Guid QuotationRequestHeaderID { get; set; }
        
        public int? TotalPoamount { get; set; }
        public DateTime? RequestedDeliveryDate { get; set; }
        public DateTime PodateTime { get; set; }
        public Guid? UserID { get; set; }
        public bool IsDeliver { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public Guid? PaymentMethodID { get; set; }
        public decimal? TaxAmount { get; set; }
        public bool IsEnter { get; set; }
        public bool IsActive { get; set; }
        //public bool IsDelivered { get; set; }
        //public List<Guid> ItemID { get; set; }
        public Guid? SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }

        public IEnumerable<QuotationRequestDetails> QuotationRequestDetails { get; set; }
        public IEnumerable<Podetail> Podetail { get; set; }
        public ICollection<PoDetailResource> PoReportDetails { get; set; }
    }
}
