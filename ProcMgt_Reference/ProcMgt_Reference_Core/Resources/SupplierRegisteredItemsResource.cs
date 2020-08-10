using System;
using System.Collections.Generic;
using System.Text;

namespace ProcMgt_Reference_Core.Resources
{
    public class SupplierRegisteredItemsResource
    {

        public string itemTypeName { get; set; }

        public Guid SupplierRegisteredItemsID { get; set; }
        public Guid ItemTypeID { get; set; }
        public string MinimumItemCapacity { get; set; }
        public double SupplierLeadTime { get; set; }
        public Guid SupplierID { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? EntryDateTime { get; set; }
    }
}
