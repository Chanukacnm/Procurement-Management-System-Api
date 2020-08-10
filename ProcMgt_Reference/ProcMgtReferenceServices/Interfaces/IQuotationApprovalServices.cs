using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProcMgt_Reference_Services.Interfaces
{
    public interface IQuotationApprovalServices
    {
        Task<IEnumerable<QuotationRequestHeader>> GetAllAsync();
        Task<DataGridTable> GetQuotationApprovalGridAsync();
    }
}
