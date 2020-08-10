using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class SupplierResource
    {
        public Guid SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string BrNo { get; set; }
        public string Address { get; set; }
        public double Telephone { get; set; }
        public string BillingName { get; set; }
        public string BillingAddress { get; set; }
        public Guid BankID { get; set; }
        public Guid BranchID { get; set; }
        public Guid AccountTypeID { get; set; }
        public double AccountNo { get; set; }
        public string AccountName { get; set; }
        public Guid PaymentMethodID { get; set; }
        public Guid SupplierTypeID { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? EntryDateTime { get; set; }

        public List<ContactDetails> ContactDetails { get; set; }
        public IEnumerable<SupplierRegisteredItems> SupplierRegisteredItems { get; set; }


    }
}
