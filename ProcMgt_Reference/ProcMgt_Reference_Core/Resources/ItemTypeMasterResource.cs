using ProcMgt_Reference_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Core.Resources
{
    public class ItemTypeMasterResource
    {
        public Guid ItemTypeID { get; set; }
        public string ItemTypeName { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryID { get; set; }
        public string ItemTypeCode { get; set; }
        public double? DepreciationRate { get; set; }
        public string MeasurementUnitName { get; set; }
        public Guid MeasurementUnitID { get; set; }
        public string PatternName { get; set; }
        public Guid ApprovalPatternTypeID { get; set; }
        public bool IsDisposable { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public string Disposable { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public bool? IsTansactions { get; set; } //--- Add By Nipuna Francisku







    }
}
