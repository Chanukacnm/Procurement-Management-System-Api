using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IReportServices
    {
     
        Task<IEnumerable<ItemRequestResource>> GetItemReqData(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<ItemRequestResource>> GetApprovedItemReqData(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<ArnheaderResource>> GetGRNData(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<ItemRequestResource>> GetReconciliationData(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync(); //rajitha
       // Task<IEnumerable<QuotationRequestHeaderResource>> GetPONumbersAsync( QuotationRequestHeaderResource QuotationRequestHeaderResource); //rajitha

    }
}
 