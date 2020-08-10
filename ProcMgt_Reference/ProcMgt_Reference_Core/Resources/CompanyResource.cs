using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Core.Resources
{
    public class CompanyResource
    {
        public Guid CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public bool? IsGroupofCompany { get; set; }
        public string CompanyStatus { get; set; }
        public Guid? CompanyLogoID { get; set; }
        public string UploadFileName { get; set; }
        public string CompanyAddressLine1 { get; set; }
        public string CompanyAddressLine2 { get; set; }
        public string CompanyAddressLine3 { get; set; }
        public string CompanyAddressLine4 { get; set; }
        public double? CompanyTelephoneNo { get; set; }
        public string CompanyFax { get; set; }
        public string Email { get; set; }
        public string CompanyWeb { get; set; }
        public string CompanyRegistrationNo { get; set; }
        public string VatRegistrationNo { get; set; }

        public List<GroupCompany> GroupCompany { get; set; }
    }
}
