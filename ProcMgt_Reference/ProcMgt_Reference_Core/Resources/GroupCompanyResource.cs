using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class GroupCompanyResource
    {
        public Guid GroupCompanyID { get; set; }
        public string GroupCompanyName { get; set; }
        public string GroupCompanyCode { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public Guid? GroupCompanyLogoID { get; set; }
        public string GroupUploadFileName { get; set; }
        public string GroupCompanyAddressLine1 { get; set; }
        public string GroupCompanyAddressLine2 { get; set; }
        public string GroupCompanyAddressLine3 { get; set; }
        public string GroupCompanyAddressLine4 { get; set; }
        public double? GcompanyTelephoneNo { get; set; }
        public string GcompanyFax { get; set; }
        public string GcompanyEmail { get; set; }
        public string GcompanyWeb { get; set; }
        public string GcompanyRegistrationNo { get; set; }
        public string VatRegistrationNo { get; set; }
        public Guid? CompanyID { get; set; }
    }
}
