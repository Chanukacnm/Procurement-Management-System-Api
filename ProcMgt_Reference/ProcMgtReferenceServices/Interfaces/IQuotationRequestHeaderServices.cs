using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IQuotationRequestHeaderServices
    {
        Task<IEnumerable<QuotationRequestHeader>> GetAllAsync();
        Task<GenericSaveResponse<QuotationRequestHeader>> SaveQuotationRequestHeaderAsync(QuotationRequestHeader quotationrequestheader);
        Task<GenericSaveResponse<QuotationRequestHeader>> UpdateQuotationRequestHeaderAsync(string id, QuotationRequestHeader quotationrequestheader);
        Task<GenericSaveResponse<QuotationRequestHeader>> UpdateQuotationRequestDetailsrAsync(string id, QuotationRequestHeader quotationrequestheader);
        Task<GenericSaveResponse<QuotationRequestHeader>> DeleteQuotationRequestHeaderAsync(string id, QuotationRequestHeader quotationrequestheader);


        Task<DataGridTable> GetQuotationRequestHeaderGridAsync();
    }
}
