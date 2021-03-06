﻿using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class QuotationRequestHeaderResource
    {
        public string QuotationNumber { get; set; }

        public DateTime QuotationRequestedDate { get; set; }
        public string SupplierName { get; set; }
        public string UserName { get; set; }


        public Guid QuotationRequestHeaderID { get; set; }
        public Guid? SupplierID { get; set; }
        public bool? IsEnteringCompleted { get; set; }
        public DateTime? RequiredDate { get; set; }
        
        public Guid UserID { get; set; }
        public int QuotationRequestStatusID { get; set; }

        public bool IsDelivered { get; set; } 




        public string QuotationCompleted { get; set; }
        public string QuotationRequestStatus1 { get; set; }
        public string ApprovalComment { get; set; }

        public IEnumerable<QuotationRequestDetails> QuotationRequestDetails { get; set; }







    }
}
